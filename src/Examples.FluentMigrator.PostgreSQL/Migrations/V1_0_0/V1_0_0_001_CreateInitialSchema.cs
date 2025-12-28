using FluentMigrator;

namespace Examples.FluentMigrator.PostgreSQL.Migrations.V1_0_0;

[Migration(20251229001, "v1.0.0.001 - Create Initial Schema")]
public class V1_0_0_001_CreateInitialSchema : Migration
{
    public override void Up()
    {
        Create.Schema("migration_example");
    }

    public override void Down()
    {
        Delete.Schema("migration_example");
    }
}
