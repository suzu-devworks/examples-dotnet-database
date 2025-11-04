using System.Data.Common;
using Dapper;
using Examples.Dapper.PostgreSQL.LearnDapper;
using Microsoft.Extensions.DependencyInjection;

namespace Examples.Dapper.PostgreSQL.Tests.LearnDapper.Dialect;

public class DateTimeMappingTests(
    LearnDapperFixtures fixture,
    ITestOutputHelper output)
    : IClassFixture<LearnDapperFixtures>
{
    public static bool IsDBAvailable => DatabaseEnvironment.IsAvailable;

    private readonly DbDataSource _dataSource = fixture
            .UseLogger(output.WriteLine)
            .ServiceProvider.GetRequiredKeyedService<DbDataSource>(DataSourceKeys.LearnDapper);


    [Theory(Skip = "DB is unavailable", SkipUnless = nameof(IsDBAvailable))]
    [MemberData(nameof(DateTimeKindSpecifiedData))]
    public async Task VerifyThatMapping_WhenDateTimeKindSpecified(DateTime date, string expectedType, DateTimeKind expectedKind)
    {
        using var connection = await _dataSource.OpenConnectionAsync(TestContext.Current.CancellationToken);
        using var transaction = await connection.BeginTransactionAsync(TestContext.Current.CancellationToken);

        var sql = """
            SELECT 
                @date AS date_original,
                CAST(pg_typeof(@date) as TEXT) AS date_type,
                CAST(pg_typeof(CURRENT_TIMESTAMP) as TEXT) AS CURRENT_TIMESTAMP_TYPE
            """;

        var actual = await connection.QuerySingleAsync(
            new CommandDefinition(
                sql,
                new { date },
                cancellationToken: TestContext.Current.CancellationToken));

        Assert.Equal(expectedType, actual.date_type);
        Assert.Equal(expectedKind, actual.date_original.Kind);
    }

    public static TheoryData<DateTime, string, DateTimeKind> DateTimeKindSpecifiedData => [
        (DateTime.Parse("2025-11-01T12:34:56"), "timestamp without time zone", DateTimeKind.Unspecified),
        (DateTime.Parse("2025-11-01T12:34:56+09:00").ToLocalTime(), "timestamp without time zone", DateTimeKind.Unspecified),
        (DateTime.Parse("2025-11-01T12:34:56Z").ToUniversalTime(), "timestamp with time zone", DateTimeKind.Utc),
    ];


}
