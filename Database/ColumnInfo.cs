using System;
using System.Reflection;

namespace Database;

/// <summary>
/// Reflection-based information on a member's <see cref="PropertyInfo"/>
/// </summary>
internal class ColumnInfo
{
    /// <summary>
    /// Name of column in database
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Is this column the primary key?
    /// </summary>
    public bool IsPrimary { get; set; }
    
    /// <summary>
    /// The property info associated with column, so we can easily set/get values via reflection
    /// </summary>
    public PropertyInfo Property { get; set; }

    /// <summary>
    /// Is this column a foreign key
    /// </summary>
    public bool IsForeignKey => ForeignTable is not null;
    
    /// <summary>
    /// Associated table this column links to / references
    /// </summary>
    public Type? ForeignTable { get; set; }
    
    /// <summary>
    /// Name of column in <see cref="ForeignTable"/>
    /// </summary>
    public string ForeignColumnName { get; set; }
    
    /// <summary>
    /// Maximum length of column (optional, for text fields)
    /// </summary>
    public int? MaxLength { get; set; }
    
    /// <summary>
    /// Is this column nullable
    /// </summary>
    public bool IsNullable { get; set; }
}