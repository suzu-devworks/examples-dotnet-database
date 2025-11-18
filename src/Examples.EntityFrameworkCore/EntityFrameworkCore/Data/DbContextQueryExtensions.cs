using Microsoft.EntityFrameworkCore;

namespace Examples.EntityFrameworkCore.Data;

public static class DbContextQueryExtensions
{
    public static async Task<List<T>> QueryRawSqlAsync<T>(this DbContext db, string sql, object[]? parameters = null, CancellationToken cancellationToken = default)
        where T : class
    {
        parameters ??= [];

        if (typeof(T).GetProperties().Length != 0)
        {
            return await db.Set<T>().FromSqlRaw(sql, parameters).ToListAsync(cancellationToken);
        }
        else
        {
            await db.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
            return default!;
        }
    }

}
