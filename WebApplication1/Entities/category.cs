using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Entities
{
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
