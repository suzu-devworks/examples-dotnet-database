using FluentMigrator;

namespace Examples.FluentMigrator.PostgreSQL.Migrations.V1_0_0;

[Migration(20251229002, "v1.0.0.002 - Create Tables")]
public class V1_0_0_002_CreateTables : Migration
{
    public override void Up()
    {
        // 1. customers
        Create.Table("customers").InSchema(Schemas.Default)
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("email").AsString(255).NotNullable().Unique()
            .WithColumn("created_at").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);

        // 2. products
        Create.Table("products").InSchema(Schemas.Default)
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("product_name").AsString(100).NotNullable()
            .WithColumn("price").AsDecimal(18, 2).NotNullable()
            .WithColumn("stock_quantity").AsInt32().NotNullable().WithDefaultValue(0);

        // 3. orders
        Create.Table("orders").InSchema(Schemas.Default)
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("customer_id").AsInt32().NotNullable()
                .ForeignKey("fk_orders_customers", Schemas.Default, "customers", "id")
            .WithColumn("order_date").AsDateTime().NotNullable()
            .WithColumn("total_amount").AsDecimal(18, 2).NotNullable();

        // 4. order_details
        Create.Table("order_details").InSchema(Schemas.Default)
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("order_id").AsInt32().NotNullable()
                .ForeignKey("fk_order_details_orders", Schemas.Default, "orders", "id")
            .WithColumn("product_id").AsInt32().NotNullable()
                .ForeignKey("fk_order_details_products", Schemas.Default, "products", "id")
            .WithColumn("quantity").AsInt32().NotNullable()
            .WithColumn("unit_price").AsDecimal(18, 2).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("OrderDetails").InSchema(Schemas.Default);
        Delete.Table("Orders").InSchema(Schemas.Default);
        Delete.Table("Products").InSchema(Schemas.Default);
        Delete.Table("Customers").InSchema(Schemas.Default);
    }

}
