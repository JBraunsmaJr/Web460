using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Database.Attributes;

namespace Database;

using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;

/// <summary>
/// Framework to use between labs and course project
/// </summary>
public class BadgerDbContext : IDisposable
{
    internal static readonly ConcurrentDictionary<Type, TableInfo> _tables = new();
    internal static bool Initialized;

    private const string DROP_TABLE_FORMAT = "DROP TABLE IF EXISTS {0}";

    private readonly SQLiteConnection _connection;

    public BadgerDbContext(string connectionString)
    {
        _connection = new SQLiteConnection(connectionString);

        _connection.Open();
    }
    
    /// <summary>
    /// Create tables for the database
    /// </summary>
    /// <typeparam name="TMarker"></typeparam>
    /// <param name="cleanBuild">Should existing tables be removed and recreated? Data will be lost</param>
    public void InitializeDatabase<TMarker>(bool cleanBuild=false)
    {
        var visibleTypes = typeof(TMarker).Assembly.GetExportedTypes();

        // Cannot be abstract, nor an interface, nor an enum
        foreach (var type in visibleTypes)
            if(!type.IsAbstract && !type.IsInterface && !type.IsEnum)
                ProcessTableType(type);

        if (!_tables.Any())
            throw new InvalidOperationException($"Failed to discover classes with {nameof(TableAttribute)} within {typeof(TMarker).Name}'s assembly.");

        using var command = new SQLiteCommand(_connection);
        
        foreach(var pair in _tables.OrderByDescending(x=>x.Value.ForeignKeys.Length))
        {
            if (cleanBuild)
            {
                command.CommandText = string.Format(DROP_TABLE_FORMAT, pair.Value.TableName);
                command.ExecuteNonQuery();    
            }
            
            // Recreate the table
            command.CommandText = Sql.CreateTableStatement(pair.Value);
            command.ExecuteNonQuery();
        }

        Initialized = true;
    }

    public DataTable GetAllAsDataTable<T>()
    {
        if (!_tables.TryGetValue(typeof(T), out var tableInfo))
            return null;
        
        var table = new DataTable();
        table.TableName = typeof(T).Name + "s";

        foreach (var col in tableInfo.Columns)
            table.Columns.Add(new DataColumn(col.Name, col.Property.PropertyType));

        var items = GetAll<T>().ToList();
        foreach (var item in items)
        {
            var dr = table.NewRow();
            dr.BeginEdit();
            foreach (var prop in tableInfo.Columns)
            {
                var value = prop.Property.GetValue(item, null);

                if (value != null)
                    dr[prop.Name] = value;
            }
                    
            dr.EndEdit();
            table.Rows.Add(dr);
        }

        return table;
    }

    /// <summary>
    /// Output Table of <typeparamref name="T"/> into XML format.
    /// </summary>
    /// <remarks>
    ///     File name will be <paramref name="basePath"/> + name of <typeparamref name="T"/> + .xml
    ///     "basePath/TName.xml"
    /// </remarks>
    /// <param name="basePath">Directory path on disk to save to</param>
    /// <param name="refreshFromDb"></param>
    /// <typeparam name="T">Table Type</typeparam>
    /// <returns>DataSet</returns>
    public DataSet GetAllAsXML<T>(string basePath, bool refreshFromDb=false)
    {
        var filePath = Path.Combine(basePath, $"{typeof(T).Name}.xml");
        
        // Prob could save performance by not doing this but in effort of time
        // doesn't really matter for a lab
        var table = GetAllAsDataTable<T>();

        var dataset = new DataSet(nameof(T));
        dataset.Tables.Add(table);
        
        if (File.Exists(filePath) && !refreshFromDb)
        {
            try
            {
                dataset.Clear();
                dataset.ReadXml(filePath);
            }
            catch (Exception ex)
            {
                // pretend we have a logging mechanism somewhere
                // log file/permission error for destination
                throw;
            }
        }
        else
        {
            try
            {
                
                
                dataset.WriteXml(filePath);
                dataset.ReadXml(filePath);
            }
            catch (Exception ex)
            {
                // pretend we have a logging mechanism somewhere
                // log error during output
                throw;
            }
        }

        return dataset;
    }

    /// <summary>
    /// Collect information about <paramref name="type"/> Table
    /// </summary>
    /// <param name="type"></param>
    private void ProcessTableType(Type type)
    {
        var tableAttribute = type.GetCustomAttribute<TableAttribute>();

        if (tableAttribute is null)
            return;

        var properties = type.GetProperties();

        List<ColumnInfo> cols = new();
        List<ColumnInfo> fks = new();
        
        foreach (var prop in properties)
        {
            var colInfo = new ColumnInfo();

            int? maxLength = null;
            string colName = prop.Name;

            var dbCol = prop.GetCustomAttribute<DbColumnAttribute>();

            if (dbCol != null)
            {
                maxLength = dbCol.Length;
                colName = dbCol.Name;
            }
            else
            {
                var maxLengthAttribute = prop.GetCustomAttribute<MaxLengthAttribute>();
                if (maxLengthAttribute != null)
                    maxLength = maxLengthAttribute.Length;
            }

            var requiredAttribute = prop.GetCustomAttribute<RequiredAttribute>();

            colInfo.IsNullable = requiredAttribute == null;
            colInfo.Name = colName;
            colInfo.MaxLength = maxLength;           
            colInfo.Property = prop;
            colInfo.IsPrimary = prop.GetCustomAttribute<IdAttribute>() is not null;
            var fk = prop.GetCustomAttribute<ForeignKeyAttribute>();

            if (fk is not null)
            {
                colInfo.ForeignTable = fk.Type;
                colInfo.ForeignColumnName = fk.Name;
                fks.Add(colInfo);
            }
            
            cols.Add(colInfo);
        }

        TableInfo tableInfo = new()
        {
            TableName = !string.IsNullOrWhiteSpace(tableAttribute.Name) ? tableAttribute.Name : type.Name,
            Columns = cols.ToArray(),
            ForeignKeys = fks.ToArray()
        };

        _tables.TryAdd(type, tableInfo);
    }

    /// <summary>
    /// Insert record into it's appropriate table, return the identity result
    /// </summary>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Insert<T>(T item)
    {
        var results = Validator.Validate(item);
        
        if(results.Any())
            throw new ValidationException(string.Join(", ", results.Select(x => x.ErrorMessage)));

        var data = Sql.CreateInsertStatement<T>();

        using var command = new SQLiteCommand(_connection);

        command.CommandText = data.Statement;

        foreach(var col in data.Parameters)
            command.Parameters.AddWithValue(col.Key, col.Value.GetValue(item));

        // This will retrieve the Identity value from insert
        var idObject = command.ExecuteScalar();

        // If the item contains a primary key, we'll update that automatically
        if (data.IdentityProperty != null)
            data.IdentityProperty.SetValue(item, idObject);

        return item;
    }

    /// <summary>
    /// Update record in it's appropriate table, if applicable. Return result
    /// </summary>
    /// <param name="item"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Update<T>(T item)
    {
        var data = Sql.CreateUpdateStatement<T>();

        using var command = new SQLiteCommand(_connection);
        command.CommandText = data.Statement;

        foreach (var col in data.Parameters)
            command.Parameters.AddWithValue(col.Key, col.Value.GetValue(item));

        command.ExecuteNonQuery();

        return item;
    }

    /// <summary>
    /// Insert <paramref name="items"/> into appropriate table, return result set with populated identity column
    /// </summary>
    /// <param name="items"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Records with primary key generated</returns>
    public IEnumerable<T> Insert<T>(IEnumerable<T> items)
    {
        foreach (var record in items)
            Insert(record);

        return items;
    }

    /// <summary>
    /// Remove record from database by primary key
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Pk"></typeparam>
    /// <returns>True: if item was deleted. Otherwise false</returns>
    public bool Delete<T>(T item)
    {
        var data = Sql.CreateDeleteStatement<T>();

        using var command = new SQLiteCommand(_connection);
        command.CommandText = data.Statement;
        command.Parameters.AddWithValue(data.ParameterName, data.PropertyInfo.GetValue(item));

        int records = command.ExecuteNonQuery();

        return records > 0;
    }

    /// <summary>
    /// Search for record in <typeparamref name="T"/> Table, by primary key
    /// </summary>
    /// <typeparam name="T">Type of object to query</typeparam>
    /// <param name="id">Primary key value to look for</param>
    /// <returns>Instance of <typeparamref name="T"/> if found. Otherwise null/default</returns>
    /// <exception cref="NotSupportedException">When a column could not be translated</exception>
    public T FindById<T>(object id)
    {
        if (!_tables.TryGetValue(typeof(T), out var table))
            return default;

        var preparedResult = table.FindById(id);

        if (preparedResult == null)
            return default;

        using var command = new SQLiteCommand(preparedResult.Statement, _connection);

        foreach (var parameter in preparedResult.Parameters)
            command.Parameters.AddWithValue(parameter.Key, parameter.Value);

        using var reader = command.ExecuteReader();

        if (!reader.HasRows)
            return default;

        reader.Read();
        var instance = ProcessRecord<T>(reader, table);

        return instance;
    }

    /// <summary>
    /// Find records of <typeparamref name="T"/> via the where clause generated via <paramref name="expression"/>
    /// </summary>
    /// <typeparam name="T">Type of record</typeparam>
    /// <param name="expression">Where clause to generate via Linq</param>
    /// <returns>Instance of <typeparamref name="T"/> if found. Otherwise default/null</returns>
    /// <exception cref="NotSupportedException"></exception>
    public T[] Find<T>(Expression<Func<T, object>> expression)
    {
        if (!_tables.TryGetValue(typeof(T), out var table))
            return Array.Empty<T>();

        List<T> results = new();

        var preparedResult = table.ProcessExpression(expression);
        using var command = new SQLiteCommand(preparedResult.Statement, _connection);

        foreach (var parameter in preparedResult.Parameters)
            command.Parameters.AddWithValue(parameter.Key, parameter.Value);

        using var reader = command.ExecuteReader();

        while (reader.Read())
            results.Add(ProcessRecord<T>(reader, table));
        
        return results.ToArray();
    }

    private static T ProcessRecord<T>(SQLiteDataReader reader, TableInfo table)
    {
        var instance = Activator.CreateInstance<T>();
        for (var i = 0; i < table.Columns.Length; i++)
        {
            var col = table.Columns[i];
            
            if(col.Property.IsOneOf(typeof(int), typeof(int?)))
                col.Property.SetValue(instance, reader.GetInt32(i));
            else if (col.Property.IsOneOf(typeof(double), typeof(double?), typeof(long), typeof(long?)))
                col.Property.SetValue(instance, reader.GetDouble(i));
            else if(col.Property.PropertyType == typeof(string))
                col.Property.SetValue(instance, reader.GetString(i));
            else if (col.Property.IsOneOf(typeof(DateTime), typeof(DateTime?)))
                col.Property.SetValue(instance, reader.GetDateTime(i));
            else
                throw new NotSupportedException($"{col.Property.Name} of type {col.Property.PropertyType} could not be translated");
        }

        return instance;
    }

    public IEnumerable<T> GetAll<T>()
    {
        if (!_tables.TryGetValue(typeof(T), out var table))
            return Array.Empty<T>();
        
        List<T> results = new();
        using var command = new SQLiteCommand($"SELECT * FROM {table.TableName}", _connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
            results.Add(ProcessRecord<T>(reader, table));
        
        return results;
    }

    /// <summary>
    /// Returns the first record to meet the defined <paramref name="expression"/>
    /// </summary>
    /// <typeparam name="T">Type of record to query</typeparam>
    /// <param name="expression">Where clause to generate</param>
    /// <returns>Instance of <typeparamref name="T"/> if found, otherwise default/null</returns>
    /// <exception cref="NotSupportedException">When a column could not be translated</exception>
    public T FirstOrDefault<T>(Expression<Func<T, object>> expression)
    {
        if (!_tables.TryGetValue(typeof(T), out var table))
            return default;

        var preparedResult = table.ProcessExpression(expression);

        using var command = new SQLiteCommand(_connection);
        command.CommandText = preparedResult.Statement;

        foreach (var parameter in preparedResult.Parameters)
            command.Parameters.AddWithValue(parameter.Key, parameter.Value);

        using var reader = command.ExecuteReader();
        
        if(!reader.HasRows)
            return default;
        
        reader.Read();
        var instance = ProcessRecord<T>(reader, table);

        return instance;
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}