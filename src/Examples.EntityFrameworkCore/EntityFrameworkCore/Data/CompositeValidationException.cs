using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Examples.EntityFrameworkCore.Data;

public class CompositeValidationException(string? message, IEnumerable<ValidationResult> errors)
    : ValidationException(message)
{
    public IEnumerable<ValidationResult> Errors { get; } = errors;

    public override string ToString()
    {
        var message = new StringBuilder(GetType().FullName);
        message.Append(' ');
        message.Append(Message);

        foreach (var error in Errors)
        {
            message.AppendLine();
            message.Append('\t');
            message.Append(error.ErrorMessage);
        }

        return message.ToString();
    }
}
