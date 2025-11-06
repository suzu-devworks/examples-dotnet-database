using System.Data.Common;
using Dapper;
using Examples.Dapper.PostgreSQL.LearnDapper;
using Examples.Dapper.PostgreSQL.LearnDapper.Models;
using Microsoft.Extensions.DependencyInjection;
using Sqlify;
using Sqlify.Core;
using Sqlify.Core.Expressions;
using Sqlify.Dapper;

namespace Examples.Dapper.PostgreSQL.Tests.LearnDapper.SqlBuilders;

public class SqlifyDapperTests(
    LearnDapperFixtures fixture,
    ITestOutputHelper output)
    : IClassFixture<LearnDapperFixtures>
{
    public static bool IsDBAvailable => DatabaseEnvironment.IsAvailable;

    private readonly DbDataSource _dataSource = fixture
            .UseLogger(output.WriteLine)
            .ServiceProvider.GetRequiredKeyedService<DbDataSource>(DataSourceKeys.LearnDapper);

    [Table("products")]
    public interface IProductTable : ITable
    {
        [Column("product_id")]
        Column<int> ProductId { get; }

        [Column("name")]
        Column<string> ProductName { get; }

        [Column("unit_price")]
        Column<decimal> UnitPrice { get; }

        [Column("category_id")]
        Column<int> CategoryId { get; }

        [Column("discontinued")]
        Column<bool> Discontinued { get; }
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task WhenManyToOneRelationships()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var tab = Sql.Table<IProductTable>("tab");

        var query = Sql
            .Select(tab.CategoryId, tab.ProductName, tab.UnitPrice)
            .From(tab)
            .Where(tab.CategoryId == 5, tab.Discontinued == false)
            .OrderByDesc(tab.UnitPrice)
            .OrderBy(tab.ProductId);

        var writer = new DapperSqlWriter();
        query.Format(writer);

        var products = (await connection.QueryAsync<Product>(
                new CommandDefinition(writer.GetCommand(), writer.GetParameters(),
                cancellationToken: TestContext.Current.CancellationToken)
                )).ToList();

        Assert.Equal(18, products.Count);
        Assert.Equal(32, products[0].UnitPrice);
        Assert.Equal(56, products[0].ProductID);
    }
}
