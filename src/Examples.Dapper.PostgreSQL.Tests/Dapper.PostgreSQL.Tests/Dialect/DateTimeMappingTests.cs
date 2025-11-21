using System.Data.Common;
using Dapper;
using Examples.Dapper.Data.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL.Tests.Dialect;

public class DateTimeMappingTests(
    DialectFixtures fixture,
    ITestOutputHelper output)
    : IClassFixture<DialectFixtures>
{
    public static bool IsDBAvailable => DatabaseEnvironment.IsAvailable;

    private readonly DbDataSource _dataSource = fixture
            .UseLogger(output.WriteLine)
            .ServiceProvider.GetRequiredService<DbDataSource>();

    private record class DateTimeMapping<T>(T Value, string? DbTypeName);

    [Theory(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    [MemberData(nameof(DateTimeKindSpecifiedData))]
    public async Task When_DateTimeKindIsChanged_Then_DatabaseRecognizedTypeIsDifferent(DateTime value, DateTimeKind expectedKind, string expectedDbType)
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);

        var actual = await connection.QuerySingleAsync<DateTimeMapping<DateTime>>(
            new CommandDefinition("""
                SELECT @value AS value, CAST(pg_typeof(@value) as TEXT) AS db_type_name;
                """,
                new { value },
                cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal(value, actual.Value);
        Assert.Equal(expectedKind, actual.Value.Kind);
        Assert.Equal(expectedDbType, actual.DbTypeName);
    }

    public static TheoryData<DateTime, DateTimeKind, string> DateTimeKindSpecifiedData => [
        (DateTime.Parse("2025-11-01T12:34:56"), DateTimeKind.Unspecified, "timestamp without time zone"),
        (DateTime.Parse("2025-11-01T12:34:56+09:00").ToLocalTime(), DateTimeKind.Unspecified, "timestamp without time zone"),
        (DateTime.Parse("2025-11-01T12:34:56Z").ToUniversalTime(), DateTimeKind.Utc, "timestamp with time zone"),
    ];

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task When_DateOnlyIsConvertedByCustomTypeHandler_Then_DatabaseTypeIsCorrectlyMapped()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
        var value = DateOnly.Parse("2025-11-01");

        DateOnlyTypeHandler.Initialize();

        var actual = await connection.QuerySingleAsync<DateTimeMapping<DateOnly>>(
            new CommandDefinition("""
                SELECT @value AS value, CAST(pg_typeof(@value) as TEXT) AS db_type_name;
                """,
                new { value },
                cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal(value, actual.Value);
        Assert.Equal("date", actual.DbTypeName);
    }

    [Fact(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    public async Task When_DateTimeOffsetIsConvertedByCustomTypeHandler_Then_DatabaseTypeIsCorrectlyMapped()
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
        var value = DateTimeOffset.Parse("2025-11-01T12:34:56+09:00");

        DateTimeOffsetTypeHandler.Initialize();

        var actual = await connection.QuerySingleAsync<DateTimeMapping<DateTimeOffset>>(
            new CommandDefinition("""
                SELECT @value AS value, CAST(pg_typeof(@value) as TEXT) AS db_type_name;
                """,
                new { value },
                cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal(value, actual.Value);
        Assert.Equal("timestamp with time zone", actual.DbTypeName);
    }

}
