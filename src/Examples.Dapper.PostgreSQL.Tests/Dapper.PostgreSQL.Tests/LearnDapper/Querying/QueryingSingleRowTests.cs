using System.Data.Common;
using Dapper;
using Examples.Dapper.PostgreSQL.LearnDapper;
using Examples.Dapper.PostgreSQL.LearnDapper.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL.Tests.LearnDapper.Querying;

public class QueryingSingleRowTests(
    LearnDapperFixtures fixture,
    ITestOutputHelper output)
    : IClassFixture<LearnDapperFixtures>
{
    public static bool IsDBAvailable => DatabaseEnvironment.IsAvailable;

    private readonly DbDataSource _dataSource = fixture
            .UseLogger(output.WriteLine)
            .ServiceProvider.GetRequiredKeyedService<DbDataSource>(DataSourceKeys.LearnDapper);

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task QueryingSingleRow_WhenThereOneResult()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var sql = "SELECT * FROM products WHERE product_id = @productID";
        var command = new CommandDefinition(sql, new { productID = 1 },
            cancellationToken: TestContext.Current.CancellationToken);

        // In the case of a single item, there are no particular problems with any of the options, 
        // so "Single" seems to be the clearest in terms of intent.

        var single = await connection.QuerySingleAsync<Product>(command);
        Assert.NotNull(single);
        Assert.Equal(1, single.ProductID);

        var singleOrDefault = await connection.QuerySingleOrDefaultAsync<Product>(command);
        Assert.NotNull(singleOrDefault);
        Assert.Equal(1, singleOrDefault.ProductID);

        var first = await connection.QueryFirstAsync<Product>(command);
        Assert.NotNull(first);
        Assert.Equal(1, first.ProductID);

        var firstOrDefault = await connection.QueryFirstOrDefaultAsync<Product>(command);
        Assert.NotNull(firstOrDefault);
        Assert.Equal(1, firstOrDefault.ProductID);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task QueryingSingleRow_WhenThereNoResults()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var sql = "SELECT * FROM products WHERE product_id = @productID";
        var command = new CommandDefinition(sql, new { productID = -1 },
            cancellationToken: TestContext.Current.CancellationToken);

        var single = await Assert.ThrowsAsync<InvalidOperationException>(
                () => connection.QuerySingleAsync<Product>(command));
        Assert.Equal("Sequence contains no elements", single.Message);

        var singleOrDefault = await connection.QuerySingleOrDefaultAsync<Product>(command);
        Assert.Null(singleOrDefault);

        var first = await Assert.ThrowsAsync<InvalidOperationException>(
                () => connection.QueryFirstAsync<Product>(command));
        Assert.Equal("Sequence contains no elements", first.Message);

        var firstOrDefault = await connection.QueryFirstOrDefaultAsync<Product>(command);
        Assert.Null(firstOrDefault);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task QueryingSingleRow_WhenThereMultipleResults()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var sql = "SELECT * FROM products WHERE category_id = @categoryID";
        var command = new CommandDefinition(sql, new { categoryID = 1 },
            cancellationToken: TestContext.Current.CancellationToken);

        var single = await Assert.ThrowsAsync<InvalidOperationException>(
                () => connection.QuerySingleAsync<Product>(command));
        Assert.Equal("Sequence contains more than one element", single.Message);

        var singleOrDefault = await Assert.ThrowsAsync<InvalidOperationException>(
                () => connection.QuerySingleOrDefaultAsync<Product>(command));
        Assert.Equal("Sequence contains more than one element", singleOrDefault.Message);

        var first = await connection.QueryFirstAsync<Product>(command);
        Assert.NotNull(first);
        Assert.Equal(1, first.CategoryID);

        var firstOrDefault = await connection.QueryFirstOrDefaultAsync<Product>(command);
        Assert.NotNull(firstOrDefault);
        Assert.Equal(1, firstOrDefault.CategoryID);
    }

}
