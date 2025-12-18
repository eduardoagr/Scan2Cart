namespace Scan2Cart.Services;

public class DataProvider(FirebaseClient client):IDataProvider {

    public async Task<Product?> GetProductByIdAsync(string id,string node = "Products") {
        try {
            var snapshot = await client.Child(node).Child(id).OnceSingleAsync<ProductDto>();

            if(snapshot == null) return null;

            return new Product {
                Id = snapshot.Id,
                Name = snapshot.Name,
                Price = decimal.TryParse(snapshot.Price,out var parsedPrice) ? parsedPrice : 0,
                Description = snapshot.Description,
                ImageUrl = snapshot.ImageUrl,
                Quantity = int.TryParse(snapshot.Quantity,out var parsedQty) ? parsedQty : 0,
            };
        } catch {

            return null;
        }
    }
}
