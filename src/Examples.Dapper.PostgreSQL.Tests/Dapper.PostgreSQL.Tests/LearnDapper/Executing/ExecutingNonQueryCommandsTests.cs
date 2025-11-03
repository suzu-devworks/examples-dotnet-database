using System.Data.Common;
using Dapper;
using Examples.Dapper.PostgreSQL.LearnDapper;
using Examples.Dapper.PostgreSQL.LearnDapper.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL.Tests.LearnDapper.Executing;

public class ExecutingNonQueryCommandsTests(
    LearnDapperFixtures fixture,
    ITestOutputHelper output)
    : IClassFixture<LearnDapperFixtures>
{
    public static bool IsDBAvailable => DatabaseEnvironment.IsAvailable;

    private readonly DbDataSource _dataSource = fixture
            .UseLogger(output.WriteLine)
            .ServiceProvider.GetRequiredKeyedService<DbDataSource>(DataSourceKeys.LearnDapper);

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task ExecuteScalarAsync_ForInsertStatement()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
        using var transaction = await connection.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var product = new Product
        {
            ProductID = 0,
            CategoryID = 1,
            Name = "やいばのブーメラン",
            Description = "攻撃力16",
            UnitPrice = 384,
        };

        var sql1 = """
            INSERT INTO products (category_id, name, description, unit_price)
            VALUES (@CategoryID, @Name, @Description, @UnitPrice)
            RETURNING product_id
            """;
        var productID = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                sql1,
                product,
                cancellationToken: TestContext.Current.CancellationToken));

        var sql2 = """
            SELECT product_id, category_id, name FROM products WHERE product_id = @productID
            """;
        var actual = await connection.QueryFirstOrDefaultAsync<Product>(
            new CommandDefinition(
                sql2,
                new { productID },
                cancellationToken: TestContext.Current.CancellationToken));

        Assert.NotNull(actual);
        Assert.Equal("やいばのブーメラン", actual.Name);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task ExecuteAsync_ForUpdateStatement()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
        using var transaction = await connection.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var sql1 = """
            UPDATE products SET name = name || @suffix
            WHERE category_id = @categoryID
            """;

        var affectedRows = await connection.ExecuteAsync(
            new CommandDefinition(
                sql1,
                new { categoryID = 2, suffix = " (Updated)" },
                cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal(13, affectedRows);

        var sql2 = """
            SELECT product_id, category_id, name FROM products WHERE category_id = @categoryID
            """;
        var actual = await connection.QueryFirstOrDefaultAsync<Product>(
            new CommandDefinition(
                sql2,
                new { categoryID = 2 },
                cancellationToken: TestContext.Current.CancellationToken));

        Assert.NotNull(actual);
        Assert.EndsWith("(Updated)", actual.Name);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task ExecuteAsync_ForDeleteStatement()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
        using var transaction = await connection.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var sql1 = """
            DELETE FROM products WHERE category_id = @categoryID
            """;
        var affectedRows = await connection.ExecuteAsync(
            new CommandDefinition(
                sql1,
                new { categoryID = 3, suffix = " (Updated)" },
                cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal(5, affectedRows);

        var sql2 = """
            SELECT product_id, category_id, name FROM products WHERE category_id = @categoryID
            """;
        var actual = await connection.QueryFirstOrDefaultAsync<Product>(
            new CommandDefinition(
                sql2,
                new { categoryID = 3 },
                cancellationToken: TestContext.Current.CancellationToken));

        Assert.Null(actual);
    }

}
