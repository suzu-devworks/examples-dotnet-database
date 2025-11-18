using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Examples.EntityFrameworkCore.Data;

public class UniversalDateTimeConvertor : ValueConverter<DateTime, DateTime>
{
    public UniversalDateTimeConvertor()
        : base(
            model => model.ToUniversalTime(),
            entity => DateTime.SpecifyKind(entity, DateTimeKind.Utc).ToLocalTime()
            )
    {
    }
}
