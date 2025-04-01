using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Interfaces.IServices;
using Server.Models.Product;
using Server.Enums.ErrorCodes;
using Server.Middlewares;
using Server.DTOs.Product;
using System.Runtime.InteropServices;
using Microsoft.Identity.Client;
using System.Threading.Tasks;


namespace Server.Services
{
    public class ProductService(DatabaseContext context) : IProductService
    {
        private readonly DatabaseContext _context = context;
        public async Task CreateProduct(CreateProductDTO productInfo)
        {
            //Kiem tra xem mat hang nay da ton tai hay chua
            if (await _context.Products.AnyAsync(a => a.Name == productInfo.Name))
            {
                throw new ProductExceptions(ProductErrorCode.ProductAlreadyExist, "Product already existed");
            }

            var product = new ProductModel
            {
                Id = Guid.NewGuid(),
                Name = productInfo.Name,
                Brand = productInfo.Brand,
                Description = productInfo.Description,
                Category = productInfo.Category,
                Price = productInfo.Price,
                Image = productInfo.Image,
                Stock = productInfo.Stock
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Guid productId,
                           UpdateProductDTO model
                           )
        {
            var product = await _context.Products.FindAsync(productId) ?? throw new ProductExceptions(ProductErrorCode.ProductNotFound, "Product not found");

            var productType = typeof(ProductModel);
            var dtoType = typeof(UpdateProductDTO);

            foreach (var prop in dtoType.GetProperties())
            {
                var newValue = prop.GetValue(model);
                if (newValue != null && !(newValue is string str && string.IsNullOrWhiteSpace(str)))
                {
                    var productProp = productType.GetProperty(prop.Name);
                    productProp?.SetValue(product, newValue);
                }
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductModel> GetProductAsync(Guid productId)
        {
            var product = await _context.Products
                                        .FirstOrDefaultAsync(a => a.Id == productId)
                                        ?? throw new ProductExceptions(ProductErrorCode.ProductNotFound, "Product not found");
            return product;

        }

        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(a => a.Id == productId)
                ?? throw new ProductExceptions(ProductErrorCode.ProductNotFound, "Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductModel>> GetListProductsAsync()
        {
            var products = await _context.Products
                                  .ToListAsync();
            return products ?? throw new ProductExceptions(ProductErrorCode.UnknownError, "Unknown Error");
        }

    }
}
