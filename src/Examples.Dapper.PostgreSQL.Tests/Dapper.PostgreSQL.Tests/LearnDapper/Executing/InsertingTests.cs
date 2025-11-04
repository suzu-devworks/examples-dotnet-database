using System.Data.Common;
using Dapper;
using Examples.Dapper.PostgreSQL.Data;
using Examples.Dapper.PostgreSQL.LearnDapper;
using Examples.Dapper.PostgreSQL.LearnDapper.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL.Tests.LearnDapper.Executing;

public class InsertingTests(
    LearnDapperFixtures fixture,
    ITestOutputHelper output)
    : IClassFixture<LearnDapperFixtures>
{
    public static bool IsDBAvailable => DatabaseEnvironment.IsAvailable;

    private readonly DbDataSource _dataSource = fixture
            .UseLogger(output.WriteLine)
            .ServiceProvider.GetRequiredKeyedService<DbDataSource>(DataSourceKeys.LearnDapper);

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task InsertMultipleRows()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
        using var transaction = await connection.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var sql0 = """
            SELECT COUNT(*) FROM products WHERE category_id = @CategoryID;
            """;
        var confirmCommand = new CommandDefinition(sql0,
                new { CategoryID = 5 },
                cancellationToken: TestContext.Current.CancellationToken);
        var beforeRows = await connection.ExecuteScalarAsync<int>(confirmCommand);

        Product[] products = [
            new () { ProductID = 0, CategoryID = 5, Name = "どくけしそう", Description = "毒を治す", UnitPrice = 10 },
            new () { ProductID = 0, CategoryID = 5, Name = "まんげつそう", Description = "マヒを治す", UnitPrice = 30 },
            new () { ProductID = 0, CategoryID = 5, Name = "いのりのゆびわ", Description = "使うとMPを小回復する。まれに壊れる", UnitPrice = 2500 },
        ];

        // Passing a list doesn't insert rows in bulk.
        var sql1 = """
            INSERT INTO products (category_id, name, description, unit_price)
            VALUES (@CategoryID, @Name, @Description, @UnitPrice)
            """;
        var affectedRows = await connection.ExecuteAsync(
            new CommandDefinition(
                sql1,
                products,
                cancellationToken: TestContext.Current.CancellationToken));

        var afterRows = await connection.ExecuteScalarAsync<int>(confirmCommand);

        Assert.True(beforeRows + affectedRows == afterRows);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task InsertMultipleRows_WithMultipleValuesByDynamicParameters()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
        using var transaction = await connection.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var sql0 = """
            SELECT COUNT(*) FROM products WHERE category_id = @CategoryID;
            """;
        var confirmCommand = new CommandDefinition(sql0,
                new { CategoryID = 5 },
                cancellationToken: TestContext.Current.CancellationToken);
        var beforeRows = await connection.ExecuteScalarAsync<int>(confirmCommand);

        Product[] products = [
            new () { ProductID = 0, CategoryID = 5, Name = "どくけしそう", Description = "毒を治す", UnitPrice = 10 },
            new () { ProductID = 0, CategoryID = 5, Name = "まんげつそう", Description = "マヒを治す", UnitPrice = 30 },
            new () { ProductID = 0, CategoryID = 5, Name = "いのりのゆびわ", Description = "使うとMPを小回復する。まれに壊れる", UnitPrice = 2500 },
        ];

        var sql1 = $$"""
            INSERT INTO products (category_id, name, description, unit_price)
            VALUES
                {{string.Join(",", products.Select((_, i) => $"(@CategoryID_{i}, @Name_{i}, @Description_{i}, @UnitPrice_{i})"))}}
            """;

        var parameters = new DynamicParameters();
        foreach (var (product, i) in products.Select((x, i) => (x, i)))
        {
            parameters.Add($"@CategoryID_{i}", product.CategoryID);
            parameters.Add($"@Name_{i}", product.Name);
            parameters.Add($"@Description_{i}", product.Description);
            parameters.Add($"@UnitPrice_{i}", product.UnitPrice);
        }

        var affectedRows = await connection.ExecuteAsync(
            new CommandDefinition(
                sql1,
                parameters,
                cancellationToken: TestContext.Current.CancellationToken));

        var afterRows = await connection.ExecuteScalarAsync<int>(confirmCommand);

        Assert.True(beforeRows + affectedRows == afterRows);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task InsertMultipleRows_WithMultipleValuesByExtensions()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
        using var transaction = await connection.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var sql0 = """
            SELECT COUNT(*) FROM products WHERE category_id = @CategoryID;
            """;
        var confirmCommand = new CommandDefinition(sql0,
                new { CategoryID = 5 },
                cancellationToken: TestContext.Current.CancellationToken);
        var beforeRows = await connection.ExecuteScalarAsync<int>(confirmCommand);

        Product[] products = [
            new () { ProductID = 0, CategoryID = 5, Name = "どくけしそう", Description = "毒を治す", UnitPrice = 10 },
            new () { ProductID = 0, CategoryID = 5, Name = "まんげつそう", Description = "マヒを治す", UnitPrice = 30 },
            new () { ProductID = 0, CategoryID = 5, Name = "いのりのゆびわ", Description = "使うとMPを小回復する。まれに壊れる", UnitPrice = 2500 },
        ];

        var sql1 = $$"""
            INSERT INTO products (category_id, name, description, unit_price)
            VALUES
                {{string.Join(",", products.Select((_, i) => $"(@CategoryID_{i}, @Name_{i}, @Description_{i}, @UnitPrice_{i})"))}}
            """;

        var affectedRows = await connection.ExecuteAsync(
            new CommandDefinition(
                sql1,
                products.ToMultipleRowsParameter(),
                cancellationToken: TestContext.Current.CancellationToken));

        var afterRows = await connection.ExecuteScalarAsync<int>(confirmCommand);

        Assert.True(beforeRows + affectedRows == afterRows);
    }

}
