using System;
using System.Collections.Concurrent;
using System.Reflection;
using Database.Attributes;

namespace Database;

public static class Util
{
    private static readonly ConcurrentDictionary<Type, string> _tableNames = new();

    /// <summary>
    /// Retrieve name of table. Caches discovered result to save future lookups
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string GetTableName<T>()
    {
        if (_tableNames.TryGetValue(typeof(T), out var name))
            return name;

        var tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>();
        
        if (tableAttribute is null) 
            return typeof(T).Name;
        
        _tableNames.TryAdd(typeof(T), tableAttribute.Name);
        return tableAttribute.Name;

    }
}