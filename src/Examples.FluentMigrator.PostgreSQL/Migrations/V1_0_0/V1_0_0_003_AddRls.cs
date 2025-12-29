using FluentMigrator;

namespace Examples.FluentMigrator.PostgreSQL.Migrations.V1_0_0;

[Migration(20251229003, "v1.0.0.003 - Add RLS")]
public class V1_0_0_003_AddRls : Migration
{
    public override void Up()
    {
        Alter.Table("customers").InSchema(Schemas.Default)
            .AddColumn("tenant_id").AsGuid().NotNullable();

        Execute.Sql($"ALTER TABLE {Schemas.Default}.customers ENABLE ROW LEVEL SECURITY;");

        Execute.Sql($"""
            CREATE POLICY customer_self_service ON {Schemas.Default}.customers
            FOR ALL
            TO PUBLIC
            USING (tenant_id = current_setting('app.current_tenant_id')::uuid);
            """);
    }

    public override void Down()
    {
        Execute.Sql($"DROP POLICY IF EXISTS customer_self_service ON {Schemas.Default}.customers;");

        Execute.Sql($"ALTER TABLE {Schemas.Default}.customers DISABLE ROW LEVEL SECURITY;");

        Delete.Column("tenant_id").FromTable("customers").InSchema(Schemas.Default);
    }
}
