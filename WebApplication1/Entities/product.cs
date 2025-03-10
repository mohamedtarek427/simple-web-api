using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Entities
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
        public string? description { get; set; } // Add this property

        [ForeignKey("Category")]
        public int? categoryId { get; set; }
        public Category? Category { get; set; } // Navigation property

    }

    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string stu { get; set; } = string.Empty; // Add this if missing
        public IFormFile? Image { get; set; }  // Ensure this is present for file uploads
        public string? description { get; set; } // Add this property

        public int? categoryId { get; set; }
    }
}
