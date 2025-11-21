namespace Examples.Dapper.PostgreSQL.ProductCatalogs.Models;

public class Product
{
    public required int ProductID { get; init; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int? SupplierID { get; set; }
    public int CategoryID { get; set; }
    public decimal UnitPrice { get; set; }
    public int UnitsInStock { get; set; }
    public int UnitsOnOrder { get; set; }
    public bool Discontinued { get; set; }
    public DateTime? DiscontinuedDate { get; set; }

    public Supplier? Supplier { get; set; }
    public Category? Category { get; set; }

}
