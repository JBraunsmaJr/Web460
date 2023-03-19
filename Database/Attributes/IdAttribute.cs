using System;

namespace Database.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class IdAttribute : Attribute
{
    public bool IsIdentity { get; set; }
}