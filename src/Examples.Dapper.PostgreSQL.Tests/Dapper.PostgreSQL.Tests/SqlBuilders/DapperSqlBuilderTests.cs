using System.Data.Common;
using Dapper;
using Examples.Dapper.PostgreSQL.ProductCatalogs;
using Examples.Dapper.PostgreSQL.ProductCatalogs.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL.Tests.SqlBuilders;

public class DapperSqlBuilderTests(
    SqlBuilderFixtures fixture,
    ITestOutputHelper output)
    : IClassFixture<SqlBuilderFixtures>
{
    public static bool IsDBAvailable => DatabaseEnvironment.IsAvailable;

    private readonly DbDataSource _dataSource = fixture
            .UseLogger(output.WriteLine)
            .ServiceProvider.GetRequiredKeyedService<DbDataSource>(DataSourceKeys.ProductCatalogs);

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task SelectTemplate()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var builder = new SqlBuilder()
                .Where("category_id = @categoryID", new { categoryID = 5 })
                .Where("discontinued = @discontinued", new { discontinued = false })
                .OrderBy("unit_price DESC")
                .OrderBy("product_id");

        var selector = builder.AddTemplate("""
                SELECT product_id, name, unit_price FROM products /**where**/ /**orderby**/
                """);

        var products = (await connection.QueryAsync<Product>(
                new CommandDefinition(selector.RawSql, selector.Parameters,
                cancellationToken: TestContext.Current.CancellationToken)
                )).ToList();

        Assert.Equal(18, products.Count);
        Assert.Equal(32, products[0].UnitPrice);
        Assert.Equal(56, products[0].ProductID);
    }

}
