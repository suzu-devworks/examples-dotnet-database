using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Examples.EntityFrameworkCore.Data;

public static class DbContextValidationExtensions
{
    public static void Validate<T>(this DbContext db, T value)
    {
        var entityProps = db.GetProperties<T>();
        var modelProps = typeof(T).GetProperties();

        List<ValidationResult> errors = [];

        foreach (var props in entityProps)
        {
            var propertyInfo = modelProps.FirstOrDefault(x => x.Name == props.Name);
            object? validatingValue = null;

            if ((!props.IsPrimaryKey()) && (!props.IsColumnNullable()))
            {
                validatingValue ??= propertyInfo!.GetValue(value);
                if (validatingValue is null)
                {
                    errors.Add(new ValidationResult($"The {props.Name} is null.", [props.Name]));
                    continue;
                }
            }

            var maxLength = props.GetMaxLength();
            if (maxLength is not null)
            {
                validatingValue ??= propertyInfo!.GetValue(value);

                if ((validatingValue is string @string) && (@string.Length > maxLength))
                {
                    errors.Add(new ValidationResult($"The {props.Name} is too long. maximum length is {maxLength}.", [props.Name]));
                    continue;
                }
                else if ((validatingValue is byte[] @byteArray) && (@byteArray.Length > maxLength))
                {
                    errors.Add(new ValidationResult($"The {props.Name} is too long. maximum length is {maxLength}.", [props.Name]));
                    continue;
                }
                else
                {
                    /* no operation */
                }
            }
        }

        if (errors.Count != 0)
        {
            throw new CompositeValidationException("One or more validation errors occurred.", errors);
        }
    }

    private static IEnumerable<IProperty> GetProperties<T>(this DbContext db)
        => db.FindEntityType<T>()?.GetProperties() ?? [];

    private static IEntityType? FindEntityType<T>(this DbContext db)
        => db.Model.FindEntityType(typeof(T));

}
