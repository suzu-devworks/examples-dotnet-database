namespace Examples.Dapper.PostgreSQL.ProductCatalogs.Models;

public class Category
{
    public required int CategoryID { get; init; }
    public required string Name { get; set; }

    public ICollection<Product> Products { get; set; } = [];
}
