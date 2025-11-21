using System.Data.Common;
using System.Reflection;
using DapperExtensions;
using DapperExtensions.Mapper;
using DapperExtensions.Predicate;
using DapperExtensions.Sql;
using Examples.Dapper.PostgreSQL.ProductCatalogs;
using Examples.Dapper.PostgreSQL.ProductCatalogs.Models;
using Examples.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL.Tests.SqlBuilders;

public class DapperExtensionsTests : IClassFixture<SqlBuilderFixtures>
{
    public static bool IsDBAvailable => DatabaseEnvironment.IsAvailable;

    private readonly DbDataSource _dataSource;

    public DapperExtensionsTests(SqlBuilderFixtures fixture, ITestOutputHelper output)
    {
        // sync

        DapperExtensions.DapperExtensions.SqlDialect = new PostgreSqlDialect();

        // async

        DapperAsyncExtensions.SqlDialect = new PostgreSqlDialect();

        _dataSource = fixture
            .UseLogger(output.WriteLine)
            .ServiceProvider.GetRequiredKeyedService<DbDataSource>(DataSourceKeys.ProductCatalogs);
    }

    private class SnakeCaseColumnPluralizedAutoClassMapper<T> : ClassMapper<T>
        where T : class
    {
        // Using `PluralizedAutoClassMapper<T>` automatically pluralizes the table name even if I specify a name, 
        // but I want to use the name exactly as specified, so I will not inherit from this class.

        public SnakeCaseColumnPluralizedAutoClassMapper()
        {
            Type type = typeof(T);
            Table(PluralizedAutoClassMapper<T>.Formatting.Pluralize(type.Name));
            AutoMap();
        }

        protected override MemberMap? Map(PropertyInfo propertyInfo, MemberMap? parent = null)
        {
            // Not included in version 1.7 codes.
            // https://github.com/tmsmith/Dapper-Extensions/issues/266
            var result = new MemberMap(propertyInfo, this, parent: parent);
            if (GuardForDuplicatePropertyMap(result))
            {
                result = (MemberMap?)Properties.FirstOrDefault(p => p.Name.Equals(result.Name) && p.ParentProperty == result.ParentProperty);
            }
            else
            {
                Properties.Add(result);
            }
            return result?.Column(propertyInfo.Name.ToSnakeCase());
        }

        private bool GuardForDuplicatePropertyMap(MemberMap result)
        {
            return Properties.Any(p => p.Name.Equals(result.Name) && p.ParentProperty == result.ParentProperty);
        }
    }

    // Auto registration.

    private class ProductClassMapper : SnakeCaseColumnPluralizedAutoClassMapper<Product>
    {
        public ProductClassMapper()
        {
            // Use SnakeCaseColumnPluralizedAutoClassMapper.

            // Table("products");
            // Map(x => x.ProductID).Column("product_id").Type(System.Data.DbType.Int32).Key(KeyType.Identity);
            // Map(x => x.Name).Column("name");
            // Map(x => x.Description).Column("description");
            // Map(x => x.SupplierID).Column("supplier_id");
            // Map(x => x.CategoryID).Column("category_id");
            // Map(x => x.UnitPrice).Column("unit_price");
            // Map(x => x.UnitsInStock).Column("units_in_stock");
            // Map(x => x.UnitsOnOrder).Column("units_on_order");
            // Map(x => x.Discontinued).Column("discontinued");
            // Map(x => x.DiscontinuedDate).Column("discontinued_date");
            Map(x => x.Supplier).Ignore();
            Map(x => x.Category).Ignore();
            AutoMap();
        }
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task GetOperation()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var product = await connection.GetAsync<Product>(new { ProductID = 1 });

        Assert.NotNull(product);
        Assert.Equal(1, product.ProductID);
        Assert.Equal("かぎ", product.Name);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task GetListOperationWithPredicates()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var predicate = Predicates.Field<Product>(f => f.CategoryID, Operator.Eq, 1);
        var products = (await connection.GetListAsync<Product>(predicate)).ToList();

        Assert.Equal(17, products.Count);
        Assert.Equal(1, products[0].CategoryID);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task SimpleInsertOperation()
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
        var map = DapperAsyncExtensions.GetMap<Product>();

        // https://github.com/tmsmith/Dapper-Extensions/issues/313
        var productID = await connection.InsertAsync(product);

        var actual = await connection.GetAsync<Product>(new { ProductID = productID });
        Assert.Equal("やいばのブーメラン", actual.Name);
    }

}
