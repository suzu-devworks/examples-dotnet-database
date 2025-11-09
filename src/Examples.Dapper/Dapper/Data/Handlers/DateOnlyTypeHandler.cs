using System.Data;
using Dapper;

namespace Examples.Dapper.Data.Handlers;

public class DateOnlyTypeHandler(DbType dbType = DbType.Date)
 : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = dbType;
        parameter.Value = dbType switch
        {
            DbType.Int32 => value.Year * 10000 + value.Month * 100 + value.Day,
            DbType.Date => value,
            _ => throw new NotSupportedException(),
        };
    }

    public override DateOnly Parse(object value)
    {
        if (value is int @intValue)
        {
            var year = @intValue / 10000;
            var month = (@intValue - year * 10000) / 100;
            var day = @intValue - year * 10000 - month * 100;

            return new DateOnly(year, month, day);
        }
        else if (value is DateTime @dateTimeValue)
        {
            return DateOnly.FromDateTime(dateTimeValue);
        }

        throw new NotSupportedException();
    }

    public static void Initialize()
    {
        SqlMapper.RemoveTypeMap(typeof(DateOnly));
        SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
    }
}
