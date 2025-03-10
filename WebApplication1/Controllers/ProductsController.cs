using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using WebApplication1.Entities;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Create a new product with an uploaded image.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<int>> CreateProduct([FromForm] CreateProductDto createProductDto)
        {
            try
            {
                if (createProductDto.Image == null || createProductDto.Image.Length == 0)
                {
                    return BadRequest("Image is required.");
                }

                var uploadFolderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads");
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                var uniqueFileName = $"{Guid.NewGuid()}_{createProductDto.Image.FileName}";
                var filePath = Path.Combine(uploadFolderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await createProductDto.Image.CopyToAsync(stream);
                }

                var product = new Product
                {
                    Name = createProductDto.Name,
                    stu = createProductDto.stu,
                    ImageUrl = $"/Uploads/{uniqueFileName}",
                    categoryId = createProductDto.categoryId,
                    description = createProductDto.description
                };

                _dbContext.Product.Add(product);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Add a new category.
        /// </summary>
        [HttpPost("category")]
        public async Task<ActionResult<int>> CreateCategory([FromBody] Category categoryDto)
        {
            try
            {
                if (string.IsNullOrEmpty(categoryDto.Name))
                {
                    return BadRequest("Category name is required.");
                }

                var category = new Category { Name = categoryDto.Name };

                _dbContext.Category.Add(category);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategoryById), new { id = category.categoryId }, category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating category: {ex.Message}");
            }
        }

        /// <summary>
        /// Get a category by ID.
        /// </summary>
        [HttpGet("category/{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            try
            {
                var category = await _dbContext.Category.FindAsync(id);
                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found.");
                }
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving category: {ex.Message}");
            }
        }

        /// <summary>
        /// Get products by category ID.
        /// </summary>
        [HttpGet("category/{categoryId}/products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetByCategory(int categoryId)
        {
            try
            {
                var products = await _dbContext.Product
                                               .Where(p => p.categoryId == categoryId)
                                               .ToListAsync();

                if (products == null || products.Count == 0)
                {
                    return NotFound($"No products found for category ID {categoryId}.");
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving products: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            try
            {
                var products = await _dbContext.Product.ToListAsync();
                if (products == null || products.Count == 0)
                {
                    return NotFound("No products found.");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving products: {ex.Message}");
            }
        }

        /// <summary>
        /// Get a single product by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            try
            {
                var product = await _dbContext.Product.FindAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving product: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing product.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] Product updateProductDto)
        {
            try
            {
                var existingProduct = await _dbContext.Product.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                existingProduct.Name = updateProductDto.Name ?? existingProduct.Name;
                existingProduct.stu = updateProductDto.stu ?? existingProduct.stu;
                existingProduct.ImageUrl = updateProductDto.ImageUrl ?? existingProduct.ImageUrl;
                existingProduct.categoryId = updateProductDto.categoryId ?? existingProduct.categoryId;
                existingProduct.description = updateProductDto.description ?? existingProduct.description;

                _dbContext.Product.Update(existingProduct);
                await _dbContext.SaveChangesAsync();

                return Ok(existingProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating product: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete a product by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var existingProduct = await _dbContext.Product.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                _dbContext.Product.Remove(existingProduct);
                await _dbContext.SaveChangesAsync();

                return Ok($"Product with ID {id} deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting product: {ex.Message}");
            }
        }
    }
}
