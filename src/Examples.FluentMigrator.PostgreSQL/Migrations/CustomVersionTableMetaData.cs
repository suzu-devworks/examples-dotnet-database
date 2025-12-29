using System;
using FluentMigrator.Runner.Conventions;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.Options;

namespace Examples.FluentMigrator.PostgreSQL.Migrations;

/// <summary>
/// Custom version table metadata.
/// </summary>
/// <param name="conventionSet"></param>
/// <param name="runnerOptions"></param>
public class CustomVersionTableMetaData(
    IConventionSet conventionSet,
    IOptions<RunnerOptions> runnerOptions)
    : DefaultVersionTableMetaData(conventionSet, runnerOptions)
{
    public override string SchemaName { get; set; } = "migration";
}
