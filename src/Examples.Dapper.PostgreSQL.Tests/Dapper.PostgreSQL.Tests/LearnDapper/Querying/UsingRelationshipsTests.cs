using System.Data.Common;
using Dapper;
using Examples.Dapper.PostgreSQL.LearnDapper;
using Examples.Dapper.PostgreSQL.LearnDapper.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL.Tests.LearnDapper.Querying;

public class UsingRelationshipsTests(
    LearnDapperFixtures fixture,
    ITestOutputHelper output)
    : IClassFixture<LearnDapperFixtures>
{
    public static bool IsDBAvailable => DatabaseEnvironment.IsAvailable;

    private readonly DbDataSource _dataSource = fixture
            .UseLogger(output.WriteLine)
            .ServiceProvider.GetRequiredKeyedService<DbDataSource>(DataSourceKeys.LearnDapper);

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task UsingRelationships_WhenManyToOneRelationships()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var sql = """
            SELECT product_id, p.name, p.category_id, c.category_id, c.name
            FROM products p
            INNER JOIN categories c ON p.category_id = c.category_id
            """;

        var products = (await connection.QueryAsync<Product, Category, Product>(
            new CommandDefinition(sql,
                cancellationToken: TestContext.Current.CancellationToken)
            , (product, category) =>
            {
                product.Category = category;
                return product;
            },
            splitOn: "category_id")
        ).ToList();

        Assert.Equal(56, products.Count);
        Assert.NotNull(products[0].Category);
        Assert.True(products[0].CategoryID > 0);
        Assert.True(products[0].CategoryID == products[0].Category!.CategoryID);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task UsingRelationships_WhenOneToManyRelationships()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var sql = """
            SELECT product_id, p.name, p.category_id, c.category_id, c.name
            FROM products p
            INNER JOIN categories c ON p.category_id = c.category_id
            """;

        var categories = (await connection.QueryAsync<Product, Category, Category>(
            new CommandDefinition(sql,
                cancellationToken: TestContext.Current.CancellationToken)
            , (product, category) =>
            {
                category.Products.Add(product);
                return category;
            },
            splitOn: "category_id"))
        .GroupBy(x => x.CategoryID)
        .Select(g =>
        {
            var category = g.First();
            category.Products = [.. g.Select(p => p.Products.Single())];
            return category;
        })
        .OrderBy(x => x.CategoryID)
        .ToList();

        Assert.Equal(5, categories.Count);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task UsingRelationships_WhenMultipleRelationships()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var sql = """
            SELECT product_id, p.name, p.supplier_id, p.category_id, s.supplier_id, s.name, c.category_id, c.name
            FROM products p
            INNER JOIN suppliers s ON p.supplier_id = s.supplier_id
            INNER JOIN categories c ON p.category_id = c.category_id
            """;

        var products = (await connection.QueryAsync<Product, Supplier, Category, Product>(
            new CommandDefinition(sql,
                cancellationToken: TestContext.Current.CancellationToken)
            , (product, supplier, category) =>
            {
                product.Supplier = supplier;
                product.Category = category;
                return product;
            },
            splitOn: "supplier_id, category_id")
        ).ToList();

        var suppliers = products
            .GroupBy(p => p.SupplierID)
            .Select(g =>
            {
                var supplier = g.First().Supplier!;
                supplier.Products = [.. g];
                return supplier;
            })
            .ToList();

        var categories = products
            .GroupBy(p => p.CategoryID)
            .Select(g =>
            {
                var category = g.First().Category!;
                category.Products = [.. g];
                return category;
            })
            .ToList();

        Assert.Equal(56, products.Count);
        Assert.Equal(18, suppliers.Count);
        Assert.Equal(5, categories.Count);
    }

}
