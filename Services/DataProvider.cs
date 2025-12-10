namespace Scan2Cart.Services;

public class DataProvider(FirebaseClient client) : IDataProvider {

    public async Task<Product> GetProductByIdAsync(string Id, string node = "Products") {

        var item = await client.Child(node).Child(Id).OnceSingleAsync<Product>();

        return item;

    }
}
