namespace Examples.Dapper.PostgreSQL.LearnDapper.Models;

public class Supplier
{
    public required int SupplierID { get; init; }
    public required string Name { get; set; }

    public ICollection<Product> Products { get; set; } = [];
}
