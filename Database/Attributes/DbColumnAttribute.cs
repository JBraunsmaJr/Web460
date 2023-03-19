using System;

namespace Database;

[AttributeUsage(AttributeTargets.Property)]
public class DbColumnAttribute : Attribute
{
    public string Name { get; set; }
    public int? Length { get; set; }


    public DbColumnAttribute(string name)
    {
        Name = name;
    }

    public DbColumnAttribute(string name, int length) : this(name)
    {
        Length = length;
    }
}