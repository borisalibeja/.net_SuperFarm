using Microsoft.AspNetCore.Mvc;
using SuperFarm.Application.DTOs;
using SuperFarm.Application.Mappers;
using SuperFarm.Infrastructure.Repositories.ProductRepositories;


namespace SuperFarm.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepositories _productRepository;

        public ProductController(IProductRepositories productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDto request)
        {
            try
            {
                var product = await _productRepository.CreateProductAsync(request);
                var productDisplayDto = product.ToProductDisplayDto();

                return CreatedAtRoute(nameof(GetProductByIdAsync), new { ProductId = productDisplayDto }, productDisplayDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }

        }

        [HttpGet("{ProductId}", Name = "GetProductByIdAsync")]
        public async Task<IActionResult> GetProductByIdAsync(Guid ProductId)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(ProductId);
                if (product == null)
                {
                    return NotFound($"Product with id {ProductId} not found");
                }
                return Ok(product.ToProductDisplayDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProductByIdAsync(Guid productId, ProductUpdateDto request)
        {
            try
            {
                if (productId == Guid.Empty)
                {
                    throw new ArgumentException("Product ID is required.");
                }

                var product = await _productRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    throw new InvalidOperationException("Product not found.");
                }

                var updatedProduct = await _productRepository.UpdateProductByIdAsync(productId, request);
                return Ok(updatedProduct);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProductAsync(Guid productId)
        {
            try
            {
                if (productId == Guid.Empty)
                {
                    throw new ArgumentException("Product ID is required.");
                }

                var product = await _productRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    throw new InvalidOperationException("Product not found.");
                }

                await _productRepository.DeleteProductByIdAsync(productId);
                return Ok("Product deleted successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productRepository.GetAllProductAsync();
                return Ok(products.Select(p => p.ToProductDisplayDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}