using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Examples.EntityFrameworkCore.Metadata;

public static class DbContextExtensions
{
    public static string? GetTableName<T>(this DbContext dbContext, T? _ = default)
        => dbContext.GetTableName(typeof(T));

    public static string? GetTableName(this DbContext dbContext, Type modelType)
    {
        var entityType = dbContext.Model.FindEntityType(modelType)
            ?? throw new InvalidOperationException($"EntityType not found: Type=\"{modelType}\".");

        var fromClause = dbContext.Database.IsSchemaSupported()
            ? entityType.GetSchemaQualifiedTableName()
            : entityType.GetTableName();

        return fromClause;
    }

    private static bool IsSchemaSupported(this DatabaseFacade database)
    {
        var supported = !string.IsNullOrEmpty(database.ProviderName) &&
                    SchemaSupportedProviderNames.Contains(database.ProviderName);

        return supported;
    }

    private static readonly HashSet<string> SchemaSupportedProviderNames = [
        "Microsoft.EntityFrameworkCore.SqlServer"
    ];

    public static IReadOnlyList<IProperty> FindPrimaryKeyProperties<T>(this DbContext dbContext, T? _ = default)
        => dbContext.Model.FindEntityType<T>()?.FindPrimaryKey()?.Properties
            ?? Enumerable.Empty<IProperty>().ToList().AsReadOnly();

    public static IReadOnlyList<IProperty> FindProperties<T>(this DbContext dbContext, T? _, IReadOnlyList<string> propertyNames)
        => dbContext.Model.FindEntityType<T>()?.FindProperties(propertyNames)
            ?? Enumerable.Empty<IProperty>().ToList().AsReadOnly();

    private static IEntityType? FindEntityType<T>(this IModel model)
        => model.FindEntityType(typeof(T));

}
