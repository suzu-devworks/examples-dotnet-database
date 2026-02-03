using System.Data;
using Dapper;

namespace Examples.Dapper.Data.Handlers;

public class TimeOnlyTypeHandler(DbType dbType = DbType.Time)
 : SqlMapper.TypeHandler<TimeOnly>
{
    public override void SetValue(IDbDataParameter parameter, TimeOnly value)
    {
        parameter.DbType = dbType;
        parameter.Value = dbType switch
        {
            DbType.Time => value,
            _ => throw new NotSupportedException(),
        };
    }

    public override TimeOnly Parse(object value)
    {
        if (value is TimeOnly @timeOnly)
        {
            return @timeOnly;
        }
        else if (value is TimeSpan @timeSpan)
        {
            return TimeOnly.FromTimeSpan(timeSpan);
        }
        else if (value is DateTime @dateTimeValue)
        {
            return TimeOnly.FromDateTime(dateTimeValue);
        }
        else if (value is int @intValue)
        {
            var hour = @intValue / 10000;
            var minute = (@intValue / 100) % 100;
            var second = @intValue % 100;

            return new TimeOnly(hour, minute, second);
        }
        else
        {
            throw new NotSupportedException();
        }
    }

    public static void Initialize()
    {
        SqlMapper.RemoveTypeMap(typeof(TimeOnly));
        SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());
    }
}
