using Scan2Cart.Models;

namespace Scan2Cart.Interfaces;

public interface IDataProvider {

    Task<Product> GetProductByIdAsync(string Id, string node = "Products");
}
