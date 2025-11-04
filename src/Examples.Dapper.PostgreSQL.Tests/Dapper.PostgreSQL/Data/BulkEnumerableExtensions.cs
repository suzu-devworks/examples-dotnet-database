using System.Dynamic;
using System.Reflection;

namespace Examples.Dapper.PostgreSQL.Data;

/// <summary>
/// Extension methods for Bulk Insert Helper.
/// </summary>
/// <remarks>
/// <para>Implementation from before I was aware of <see cref="DynamicParameters" />.</para>
/// </remarks>
public static class BulkEnumerableExtensions
{
    public static dynamic ToMultipleRowsParameter<T>(this IEnumerable<T> source, Func<PropertyInfo, bool>? exclude = null)
    {
        var props = typeof(T).GetProperties()
            .Where(x => x.CanRead & x.CanWrite)
            .Where(x => !(exclude?.Invoke(x) ?? false));

        var anonymous = source
            .Select((x, i) => (x, i))
            .Aggregate(
                new ExpandoObject(),
                (accumulate, tuple) =>
                {
                    var (data, num) = tuple;

                    foreach (var prop in props)
                    {
                        var name = $"{prop.Name}_{num}";
                        var value = prop.GetValue(data);
                        accumulate.TryAdd(name, value);
                    }

                    return accumulate;
                });

        return anonymous;
    }

}
