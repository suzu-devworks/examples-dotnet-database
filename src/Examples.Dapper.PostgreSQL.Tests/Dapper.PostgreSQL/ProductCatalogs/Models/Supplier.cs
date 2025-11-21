namespace Examples.Dapper.PostgreSQL.ProductCatalogs.Models;

public class Supplier
{
    public required int SupplierID { get; init; }
    public required string Name { get; set; }

    public ICollection<Product> Products { get; set; } = [];
}
