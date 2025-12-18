namespace Scan2Cart.Models {

    public class ProductDto {

        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string Price { get; set; }

        public required string Description { get; set; }

        public required string ImageUrl { get; set; }

        public required string Quantity { get; set; }
    }
}
