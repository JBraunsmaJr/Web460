using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Database;

internal class ExpressionResult
{
    public string Statement { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
}

internal class TableInfo
{
    public string TableName { get; set; }
    public ColumnInfo[] Columns { get; set; }
    public ColumnInfo[] ForeignKeys { get; set; }

    private Dictionary<string, ColumnInfo> _cols;

    public ExpressionResult ProcessExpression<T>(Expression<Func<T,object>> expression, bool selectOne = false)
    {
        _cols ??= Columns.ToDictionary(x => x.Name, x => x);

        var parameters = new Dictionary<string, object>();

        var body = expression.Body;

        var statement = body switch
        {
            BinaryExpression bi => ProcessLogicalBinaryExpression(bi, parameters),
            ConstantExpression con => ProcessConstantExpression(con, parameters),
            MemberExpression mem => ProcessPropertyExpression(mem, parameters),
            UnaryExpression ue => ProcessUnaryExpression(ue, parameters),
            _ => string.Empty
        };

        return new ExpressionResult
        {
            Parameters = parameters,
            Statement = selectOne
                ? $"SELECT * FROM {TableName} WHERE {statement} LIMIT 1"
                : $"SELECT * FROM {TableName} WHERE {statement}"
        };
    }

    public ExpressionResult FindById(object value)
    {
        _cols ??= Columns.ToDictionary(x => x.Name, x => x);

        var identity = Columns.FirstOrDefault(x => x.IsPrimary);

        if (identity == null)
            return null;

        var parameters = new Dictionary<string, object>();
        parameters.Add(identity.Name, value);

        return new ExpressionResult
        {
            Parameters = parameters,
            Statement = $"SELECT * FROM {TableName} WHERE {identity.Name} = @{identity.Name}"
        };
    }

    private string ProcessLogicalBinaryExpression(BinaryExpression expression, Dictionary<string, object> parameters)
    {
        if (expression is null)
            return string.Empty;

        var left = string.Empty;
        var right = string.Empty;

        switch (expression.Left)
        {
            case UnaryExpression ue:
                left = ProcessUnaryExpression(ue, parameters);
                break;
            case ConstantExpression con:
                left = ProcessConstantExpression(con, parameters);
                break;
            case BinaryExpression bi:
                left = ProcessLogicalBinaryExpression(bi, parameters);
                break;
            case MemberExpression me:
                left = ProcessPropertyExpression(me, parameters);
                break;
            default:
                left = left;
                break;
        }

        switch (expression.Right)
        {
            case BinaryExpression bi:
                right = ProcessLogicalBinaryExpression(bi, parameters);
                break;
            case ConstantExpression con:
                right = ProcessConstantExpression(con, parameters);
                break;
            case UnaryExpression ue:
                right = ProcessUnaryExpression(ue, parameters);
                break;
            case MemberExpression me:
                right = ProcessPropertyExpression(me, parameters);
                break;
            default:
                right = right;
                break;
        }
        
        var nullCheck = left.Equals("null") || right.Equals("null");
        
        return expression.NodeType switch
        {
            ExpressionType.And => $"{left} AND {right}",
            ExpressionType.Or => $"{left} OR {right}",
            ExpressionType.AndAlso => $"({left} AND {right})",
            ExpressionType.OrElse => $"({left} OR {right})",
            ExpressionType.GreaterThan => $"{left} > {right}",
            ExpressionType.GreaterThanOrEqual => $"{left} >= {right}",
            ExpressionType.LessThan => $"{left} < {right}",
            ExpressionType.LessThanOrEqual => $"{left} <= {right}",
            ExpressionType.Equal => !nullCheck ? $"{left} = {right}" : $"{left} is not {right}",
            ExpressionType.NotEqual => !nullCheck ? $"{left} != {right}" : $"{left} is {right}",
            _ => string.Empty
        };
    }

    private string ProcessConstantExpression(ConstantExpression expression, Dictionary<string,object> parameters)
    {
        if (expression.Value == null)
            return "null";

        if (typeof(string) == expression.Type)
            return $"'{expression.Value}'";
        
        // if (typeof(DateTime) == expression.Type)
        //     return expression.Value.ToString("yyyy-mm-dd hh:MM:ss")
                
        return expression.Value?.ToString() ?? string.Empty;
    }

    private string ProcessUnaryExpression(UnaryExpression expression, Dictionary<string, object> parameters)
    {
        var statement = expression.Operand switch
        {
            UnaryExpression ue => ProcessUnaryExpression(ue, parameters),
            MemberExpression me => ProcessPropertyExpression(me, parameters),
            BinaryExpression bi => ProcessLogicalBinaryExpression(bi, parameters),
            ConstantExpression con => ProcessConstantExpression(con, parameters),
            _ => string.Empty
        };

        return expression.NodeType == ExpressionType.Not
            ? $"NOT({statement})"
            : statement;
    }

    private string ProcessPropertyExpression(MemberExpression expression, Dictionary<string,object> parameters)
    {
        if (expression.Expression == null)
            return expression.Member.Name;
        
        if(expression.Expression.GetType().Name == "TypedParameterExpression")
            return $"{expression.Member.Name}";
        if (expression.Expression.GetType().Name == "TypedConstantExpression")
        {
            var instanceObject = ((ConstantExpression)expression.Expression).Value;
            var parameterName = "@";
            object constantValue = null;
            
            switch (expression.Member)
            {
                case FieldInfo field:
                    constantValue = field.GetValue(instanceObject);
                    parameterName += field.Name;
                    break;
                case PropertyInfo prop:
                    constantValue = prop.GetValue(instanceObject);
                    parameterName += prop.Name;
                    break;
            }

            if (!parameters.ContainsKey(parameterName))
                parameters.Add(parameterName, constantValue);
            else
                parameters[parameterName] = constantValue;

            return parameterName;
        }

        return expression.Member.Name;
    }
}