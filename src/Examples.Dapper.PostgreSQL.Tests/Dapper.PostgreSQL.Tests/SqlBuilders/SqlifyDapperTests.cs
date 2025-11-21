using System.Data.Common;
using Dapper;
using Examples.Dapper.PostgreSQL.ProductCatalogs;
using Examples.Dapper.PostgreSQL.ProductCatalogs.Models;
using Microsoft.Extensions.DependencyInjection;
using Sqlify;
using Sqlify.Core;
using Sqlify.Core.Expressions;
using Sqlify.Dapper;
using Sqlify.Postgres;

namespace Examples.Dapper.PostgreSQL.Tests.SqlBuilders;

public class SqlifyDapperTests(
    SqlBuilderFixtures fixture,
    ITestOutputHelper output)
    : IClassFixture<SqlBuilderFixtures>
{
    public static bool IsDBAvailable => DatabaseEnvironment.IsAvailable;

    private readonly DbDataSource _dataSource = fixture
            .UseLogger(output.WriteLine)
            .ServiceProvider.GetRequiredKeyedService<DbDataSource>(DataSourceKeys.ProductCatalogs);

    // The question is, how do we generate this?
    [Table("products")]
    public interface IProductTable : ITable
    {
        [Column("product_id")]
        Column<int> ProductId { get; }

        [Column("name")]
        Column<string> Name { get; }

        [Column("description")]
        Column<string?> Description { get; }

        [Column("supplier_id")]
        Column<int> SupplierID { get; }

        [Column("category_id")]
        Column<int> CategoryId { get; }

        [Column("unit_price")]
        Column<decimal> UnitPrice { get; }

        [Column("units_in_stock")]
        Column<decimal> UnitsInStock { get; }

        [Column("units_on_order")]
        Column<decimal> UnitsOnOrder { get; }

        [Column("discontinued")]
        Column<bool> Discontinued { get; }

        [Column("discontinued_date")]
        Column<DateTime?> DiscontinuedDate { get; }
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task SelectQuery()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var tab = Sql.Table<IProductTable>("t");
        SelectQuery query = Sql
            .Select(tab.ProductId, tab.CategoryId, tab.Name, tab.UnitPrice)
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

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task InsertQuery()
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

        var u = Sql.Table<IProductTable>();
        PgInsertQuery command = PgSql
            .Insert(u)
            .Values(u.CategoryId, product.CategoryID)
            .Values(u.Name, product.Name)
            .Values(u.Description, product.Description)
            .Values(u.UnitPrice, 384)
            .Returning();

        var writer = new DapperSqlWriter();
        command.Format(writer);

        var productID = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(writer.GetCommand(), writer.GetParameters(),
                cancellationToken: TestContext.Current.CancellationToken));

        SelectQuery query = Sql
            .Select(u.ProductId, u.CategoryId, u.Name)
            .From(u)
            .Where(u.ProductId == productID);

        writer = new DapperSqlWriter();
        query.Format(writer);

        var actual = await connection.QuerySingleAsync<Product>(
            new CommandDefinition(writer.GetCommand(), writer.GetParameters(),
                cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal("やいばのブーメラン", actual.Name);
    }
}
