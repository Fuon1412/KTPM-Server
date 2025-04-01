using Server.DTOs.Product;
using Server.Models.Product;

namespace Server.Interfaces.IServices
{
    public interface IProductService
    {
        Task CreateProduct(CreateProductDTO productInfo);
        Task UpdateProduct(Guid productId,
                           UpdateProductDTO model
                           );
        Task<GetProductDTO> GetProductAsync(Guid productId);
        Task DeleteProductAsync(Guid productId);
        Task<List<ProductModel>> GetListProductsAsync();
    }
}
