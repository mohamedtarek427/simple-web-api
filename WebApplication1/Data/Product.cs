using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // Ensure a default value

        [Column("stu")]
        public string stu { get; set; } = string.Empty; // Ensure a default value

        public string? ImageUrl { get; set; } // Nullable to handle possible NULL values
        public int categoryId { get; set; }
    }

    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string stu { get; set; } = string.Empty; // Add this if missing
        public IFormFile Image { get; set; } // Ensure this is present for file uploads
        public int categoryId { get; set; }
    }
    public class Category
    {
        [Key]
        public int categoryId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
    public class CategoryDTO
    {
        public string Name { get; set; } = string.Empty;
    }
}