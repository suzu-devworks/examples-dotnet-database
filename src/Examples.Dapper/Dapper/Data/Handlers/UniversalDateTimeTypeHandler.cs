using System.Data;
using Dapper;

namespace Examples.Dapper.Data.Handlers;

public class UniversalDateTimeTypeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        // Always be set to UTC.
        parameter.Value = value.ToUniversalTime();
    }

    public override DateTime Parse(object value)
    {
        // Parse the database value and specify its kind as UTC
        return value is DateTime @datetimeValue
            ? DateTime.SpecifyKind(@datetimeValue, DateTimeKind.Utc).ToLocalTime()
            : throw new NotSupportedException($"{value.GetType().Name} is not supported.");
    }

    public static void Initialize()
    {
        SqlMapper.RemoveTypeMap(typeof(DateTime));
        SqlMapper.AddTypeHandler(new UniversalDateTimeTypeHandler());
    }
}
