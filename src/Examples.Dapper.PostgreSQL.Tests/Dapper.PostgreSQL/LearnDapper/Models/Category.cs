namespace Examples.Dapper.PostgreSQL.LearnDapper.Models;

public class Category
{
    public required int CategoryID { get; init; }
    public required string Name { get; set; }
}
