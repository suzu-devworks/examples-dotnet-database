using System.Data;
using Dapper;

namespace Examples.Dapper.Data.Handlers;

public class DateTimeOffsetTypeHandler : SqlMapper.TypeHandler<DateTimeOffset>
{
    public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
    {
        parameter.Value = value.ToUniversalTime();
    }

    public override DateTimeOffset Parse(object value)
    {
        return value is DateTime @dateTime
            ? new DateTimeOffset(@dateTime)
            : throw new NotSupportedException();
    }

    public static void Initialize()
    {
        SqlMapper.RemoveTypeMap(typeof(DateTimeOffset));
        SqlMapper.AddTypeHandler(new DateTimeOffsetTypeHandler());
    }
}
