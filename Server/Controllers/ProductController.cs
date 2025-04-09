using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Product;
using Server.Enums.ErrorCodes;
using Server.Interfaces.IServices;
using Server.Middlewares;
using Server.Models.Product;

namespace Server.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("all-products")]
        public async Task<IActionResult> GetProduct()
        {
            try
            {
                var products = await _productService.GetListProductsAsync();

                return Ok(products);
            }
            catch (ProductExceptions ex)
            {
                return ex.ErrorCode switch
                {
                    ProductErrorCode.UnknownError => BadRequest(new
                    {
                        errorCode = "P1",
                        message = "Unknown Error"
                    }),
                    _ => BadRequest(new { message = "Unexpected Error" })
                };
            }
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> CreateNewProductAsync([FromBody] CreateProductDTO productInfo)
        {
            try
            {
                await _productService.CreateProduct(productInfo);

                return Ok(new { message = "Created Product successfully" });
            }
            catch (ProductExceptions ex)
            {
                return ex.ErrorCode switch
                {
                    ProductErrorCode.UnknownError =>
                    BadRequest(new
                    {
                        erroCode = "P2",
                        message = "Unknown Error"
                    }),
                    _ => BadRequest(new { message = "Failed to create a product" })
                };
            }
        }

        [HttpDelete("/delete-product")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            try
            {
                await _productService.DeleteProductAsync(productId);
                return Ok(new { message = "Deleted Product successfully" });
            }
            catch (ProductExceptions ex)
            {
                return ex.ErrorCode switch
                {
                    ProductErrorCode.UnknownError =>
                    BadRequest(new
                    {
                        errorCode = "P3",
                        message = "Unknown Error"
                    }),
                    _ => BadRequest(new { message = "Failed to delete a product" })
                };
            }
        }

        [HttpGet("/product")]
        public async Task<IActionResult> GetAProduct(Guid id)
        {
            try
            {
                ProductModel? product = await _productService.GetProductAsync(id);

                return Ok(product);
            }
            catch (ProductExceptions ex)
            {
                return ex.ErrorCode switch
                {
                    ProductErrorCode.UnknownError => BadRequest(new
                    {
                        errorCode = "P4",
                        message = "Unknown Error"
                    }),
                    _ => BadRequest(new { message = "Unexpected Error" })
                };
            }
        }
        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateAProduct(Guid id, ProductModel model)
        {
            try
            {
                ProductModel? product = await _productService.GetProductAsync(id);

                if (product == null)
                {
                    return BadRequest(new { message = "Product not found" });
                }

                await _productService.UpdateProduct(model);
                return Ok(new { message = "Update profile successful" });

            }
            catch (ProductExceptions ex)
            {
                return ex.ErrorCode switch
                {
                    ProductErrorCode.UnknownError => BadRequest(new { message = "Unknown Error" }),
                    _ => BadRequest(new { message = "Unexpected Error" })
                };
            }
        }
    }
}
