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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(Guid id, ProductUpdateDto productUpdateDto)
        {
            try
            {
                if (id != productUpdateDto.ProductId)
                {
                    return BadRequest("Ids mismatch");
                }
                var existingProduct = await _productRepository.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound($"Product with id {id} not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(Guid id)
        {
            try
            {
                var existingProduct = await _productRepository.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound($"Product with id {id} not found");
                }
                await _productRepository.DeleteProductAsync(id);
                return Ok("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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