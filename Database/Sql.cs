using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Database;

/// <summary>
/// Data structure containing pertinent info for insert statement
/// </summary>
internal class InsertResult
{
    public string Statement { get; set; }
    
    /// <summary>
    /// Named parameters that are contained within <see cref="Statement"/>
    /// </summary>
    public Dictionary<string, PropertyInfo> Parameters { get; set; }
    
    /// <summary>
    /// The property that shall get set AFTER insert.
    /// </summary>
    public PropertyInfo IdentityProperty { get; set; }
}

/// <summary>
/// Data structure containing pertinent info for update statement
/// </summary>
internal class UpdateResult
{
    public string Statement { get; set; }
    
    /// <summary>
    /// Named parameters that are contained within <see cref="Statement"/>
    /// </summary>
    public Dictionary<string, PropertyInfo> Parameters { get; set; }
}

internal class DeleteResult
{
    /// <summary>
    /// SQL to execute
    /// </summary>
    public string Statement { get; set; }
    
    /// <summary>
    /// Name of identity column to check
    /// </summary>
    public string ParameterName { get; set; }
    
    /// <summary>
    /// The associated identity property column
    /// </summary>
    public PropertyInfo PropertyInfo { get; set; }
}

public static class Sql
{
    #region cached reflection -- speed up future queries
    private static readonly Dictionary<Type, InsertResult> InsertCache = new();
    private static readonly Dictionary<Type, UpdateResult> UpdateCache = new();
    private static readonly Dictionary<Type, DeleteResult> DeleteCache = new();
    #endregion

    private const string InsertIntoFormat = "INSERT INTO {0} ({1}) VALUES({2})";
    
    /// <summary>
    /// Generate the appropriate SQL for creating a table
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    internal static string CreateTableStatement(TableInfo table)
    {
        List<string> cols = new();
        List<string> fks = new();
        
        foreach (var col in table.Columns)
        {
            var colType = ColumnType(col);
            cols.Add(col.IsPrimary ? $"{col.Name} {colType} PRIMARY KEY AUTOINCREMENT" : $"{col.Name} {colType}");

            if (col.IsForeignKey)
                fks.Add($"CONSTRAINT {col.Name}_{col.ForeignTable.Name}_{col.ForeignColumnName} FOREIGN KEY ({col.Name}) REFERENCES {col.ForeignTable.Name}({col.ForeignColumnName})");
        }

        // The foreign key constraints shall get put at the end
        cols.AddRange(fks);
        
        return $"CREATE TABLE IF NOT EXISTS {table.TableName}({string.Join(",", cols)})";
    }
    
    internal static UpdateResult CreateUpdateStatement<T>()
    {
        if (UpdateCache.ContainsKey(typeof(T)))
            return UpdateCache[typeof(T)];

        if (!BadgerDbContext._tables.TryGetValue(typeof(T), out var table)) return null;

        var values = new Dictionary<string, PropertyInfo>();
        var sets = new List<string>();

        foreach(var col in table.Columns.Where(x=>!x.IsPrimary))
        {
            sets.Add($"{col.Name} = @{col.Name}");
            values.Add($"@{col.Name}", col.Property);
        }

        var identity = table.Columns.FirstOrDefault(x => x.IsPrimary);
        values.Add($"@{identity.Name}", identity.Property);

        var updateStatement = $"UPDATE {table.TableName} SET {string.Join(",", sets)} WHERE {identity.Name} = @{identity.Name}";
        
        var result = new UpdateResult()
        {
            Parameters = values,
            Statement = updateStatement
        };

        UpdateCache.Add(typeof(T), result);
        return result;
    }

    internal static InsertResult CreateInsertStatement<T>()
    {
        if (InsertCache.ContainsKey(typeof(T)))
            return InsertCache[typeof(T)];

        if (!BadgerDbContext._tables.TryGetValue(typeof(T), out var table)) return null;

        var values = new Dictionary<string, PropertyInfo>();

        foreach(var col in table.Columns.Where(x=>!x.IsPrimary))
            values.Add($"@{col.Name}", col.Property);

        var insertIntoStatement = string.Format(InsertIntoFormat, table.TableName, string.Join(",", values.Keys.Select(x=>x.Substring(1))), string.Join(",", values.Keys));
        
        var result = new InsertResult
        {
            IdentityProperty = table.Columns.FirstOrDefault(x => x.IsPrimary)?.Property,
            Parameters = values,
            Statement = insertIntoStatement
        };

        InsertCache.Add(typeof(T), result);
        return result;
    }

    internal static DeleteResult CreateDeleteStatement<T>()
    {
        if (DeleteCache.ContainsKey(typeof(T)))
            return DeleteCache[typeof(T)];

        if (!BadgerDbContext._tables.TryGetValue(typeof(T), out var table)) return null;

        var identity = table.Columns.FirstOrDefault(x => x.IsPrimary);

        var statement = $"DELETE FROM {table.TableName} WHERE {identity.Name} = @{identity.Name}";
        var result = new DeleteResult
        {
            Statement = statement,
            PropertyInfo = identity.Property,
            ParameterName = $"@{identity.Name}"
        };

        DeleteCache.Add(typeof(T), result);
        return result;
    }
    
    internal static bool IsOneOf(this PropertyInfo prop, params Type[] types) 
        => types.Any(type => prop.PropertyType == type);

    /// <summary>
    /// Determine the appropriate type for our column
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    private static string ColumnType(ColumnInfo col)
    {
        var nullable = col.IsNullable ? " NULL" : string.Empty;
        
        if (col.Property.PropertyType == typeof(int))
        {
            return col.IsPrimary ? "INTEGER NOT NULL" : "INT";
        }
        if (col.Property.PropertyType == typeof(int?))
            return "INT NULL";
        if (col.Property.IsOneOf(typeof(long), typeof(double)))
            return "NUMERIC";
        if (col.Property.IsOneOf(typeof(long?), typeof(double?)))
            return "NUMERIC NULL";

        if (col.Property.PropertyType == typeof(string))
            return col.MaxLength is not null ? $"VARCHAR({col.MaxLength.Value}){nullable}" : "TEXT{nullable}";

        if (col.Property.PropertyType == typeof(DateTime))
            return $"DATETIME{nullable}";
        
        return $"TEXT{nullable}";
    }
    
    
}