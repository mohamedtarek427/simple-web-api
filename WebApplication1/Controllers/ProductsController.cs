using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

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
        [Route("")]
        public async Task<ActionResult<int>> CreateProduct([FromForm] CreateProductDto createProductDto)
        {
            try
            {
                if (createProductDto.Image == null || createProductDto.Image.Length == 0)
                {
                    return BadRequest("Image is required.");
                }

                // Define the Uploads folder inside the project root
                var uploadFolderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Uploads");

                // Ensure the folder exists
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                // Generate a unique filename to prevent conflicts
                var uniqueFileName = $"{Guid.NewGuid()}_{createProductDto.Image.FileName}";
                var filePath = Path.Combine(uploadFolderPath, uniqueFileName);

                // Save the image file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await createProductDto.Image.CopyToAsync(stream);
                }

                // Store the relative image URL
                var product = new Product
                {
                    Name = createProductDto.Name,
                    stu = createProductDto.stu, // Ensure proper casing
                    ImageUrl = $"/Uploads/{uniqueFileName}"
                };

                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Upload an image separately and return the file URL.
        /// </summary>
        [HttpPost]
        [Route("upload-image")]
        public async Task<ActionResult<string>> UploadImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                // Ensure unique filenames
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var uploadFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                var filePath = Path.Combine(uploadFolderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var fileUrl = $"/images/{uniqueFileName}";
                return Ok(new { url = fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading image: {ex.Message}");
            }
        }

        /// <summary>
        /// Update an existing product.
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] Product updateProductDto)
        {
            try
            {
                var existingProduct = await _dbContext.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                existingProduct.Name = updateProductDto.Name ?? existingProduct.Name;
                existingProduct.stu = updateProductDto.stu ?? existingProduct.stu;
                existingProduct.ImageUrl = updateProductDto.ImageUrl ?? existingProduct.ImageUrl;

                _dbContext.Products.Update(existingProduct);
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
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var existingProduct = await _dbContext.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                _dbContext.Products.Remove(existingProduct);
                await _dbContext.SaveChangesAsync();

                return Ok($"Product with ID {id} deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting product: {ex.Message}");
            }
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            try
            {
                var products = await _dbContext.Products.ToListAsync();
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
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            try
            {
                var product = await _dbContext.Products.FindAsync(id);
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
    }
}
