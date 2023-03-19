using System;
using System.Linq;

namespace Database.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ForeignKeyAttribute : Attribute
{
    public string Name { get; set; }
    public Type Type { get; set; }

    public ForeignKeyAttribute(Type type, string name)
    {
        Name = name;
        Type = type;

        if (!type.GetProperties().Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException($"Argument: {nameof(type)} ({type.Name}): does not contain property with name of \"{name}\"");
    }
}