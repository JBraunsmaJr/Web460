using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database;

public class Validator
{
    public static List<ValidationResult> Validate<T>(T item)
    {
        var context = new ValidationContext(item);
        var results = new List<ValidationResult>();
        System.ComponentModel.DataAnnotations.Validator.TryValidateObject(item, context, results);

        return results;
    }
}