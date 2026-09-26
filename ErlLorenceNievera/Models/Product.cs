using System.ComponentModel.DataAnnotations;

namespace Nievera_ErlLorenceAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Product name must be between 2 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Range(0.01, 999999999,
            ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue,
            ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }
    }
}
