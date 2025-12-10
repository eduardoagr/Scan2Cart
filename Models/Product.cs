namespace Scan2Cart.Models;

public class Product {

    public required string Id { get; set; }

    public required string Name { get; set; }

    public required string Price { get; set; }

    public required string ImageUrl { get; set; }

    public required int Quantity { get; set; }
}
