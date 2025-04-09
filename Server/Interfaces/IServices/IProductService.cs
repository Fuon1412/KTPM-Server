using Server.DTOs.Product;
using Server.Models.Product;

namespace Server.Interfaces.IServices
{
    public interface IProductService
    {
        Task CreateProduct(CreateProductDTO productInfo);
        Task UpdateProduct(ProductModel model);
        Task<ProductModel> GetProductAsync(Guid productId);
        Task DeleteProductAsync(Guid productId);
        Task<List<ProductModel>> GetListProductsAsync();
    }
}
