using System.Data.Common;
using Dapper;
using Examples.Dapper.PostgreSQL.ProductCatalogs;
using Examples.Dapper.PostgreSQL.ProductCatalogs.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL.Tests.LearnDapper.Querying;

public class QueryingTests(
    LearnDapperFixtures fixture,
    ITestOutputHelper output)
    : IClassFixture<LearnDapperFixtures>
{
    public static bool IsDBAvailable => DatabaseEnvironment.IsAvailable;

    private readonly DbDataSource _dataSource = fixture
            .UseLogger(output.WriteLine)
            .ServiceProvider.GetRequiredKeyedService<DbDataSource>(DataSourceKeys.ProductCatalogs);

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task QueryingScalarValues()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var sql = "SELECT COUNT(*) FROM products";
        var count = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(sql,
                cancellationToken: TestContext.Current.CancellationToken)
        );

        Assert.Equal(56, count);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task QueryingSingleRow()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var sql = "SELECT * FROM products WHERE product_id = @productID";
        var product = await connection.QuerySingleAsync<Product>(
                new CommandDefinition(sql,
                new { productID = 1 },
                cancellationToken: TestContext.Current.CancellationToken)
                );

        Assert.NotNull(product);
        Assert.Equal(1, product.ProductID);
        Assert.Equal("かぎ", product.Name);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task QueryingMultipleRows()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var sql = "SELECT * FROM products WHERE category_id = @categoryID";
        var products = (await connection.QueryAsync<Product>(
            new CommandDefinition(sql,
                new { categoryID = 1 },
                cancellationToken: TestContext.Current.CancellationToken)
                )).ToList();

        Assert.Equal(17, products.Count);
        Assert.Equal(1, products[0].CategoryID);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task QueryingMultipleResults()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var sql = """
            SELECT * FROM categories WHERE category_id = @categoryID;
            SELECT * FROM products WHERE category_id = @categoryID;
            """;
        using var multi = await connection.QueryMultipleAsync(
                new CommandDefinition(sql,
                new { categoryID = 1 },
                cancellationToken: TestContext.Current.CancellationToken)
                );
        var category = await multi.ReadFirstAsync<Category>();
        var products = (await multi.ReadAsync<Product>()).ToList();

        Assert.NotNull(category);
        Assert.Equal("ぶき", category.Name);

        Assert.Equal(17, products.Count);
        Assert.Equal(1, products[0].CategoryID);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task QueryingSpecificColumns()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var sql = "SELECT product_id, name FROM products WHERE product_id = @productID";
        var product = await connection.QuerySingleAsync<Product>(
                new CommandDefinition(sql,
                new { productID = 1 },
                cancellationToken: TestContext.Current.CancellationToken)
                );

        Assert.NotNull(product);
        Assert.Equal(1, product.ProductID);
        Assert.Equal("かぎ", product.Name);

        // Columns that are not specified are usually Null.
        Assert.Null(product.Description);
        Assert.Equal(0, product.UnitPrice);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task QueryingUnbufferedAsync()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var sql = "SELECT * FROM products WHERE category_id = @categoryID";
        var count = 0;
        await foreach (var product in connection.QueryUnbufferedAsync<Product>(sql, new { categoryID = 1 }))
        {
            TestContext.Current.CancellationToken.ThrowIfCancellationRequested(); // safe?

            Assert.Equal(1, product.CategoryID);
            ++count;
        }

        Assert.Equal(17, count);
    }

}
