using System.ComponentModel.DataAnnotations;

namespace BuyWithOffer
{
    public class CategoryDto : CommonEntityDto
    {
        public int CategoryId { get; set; }
        public string Category { get; set; }
    }

    public class CreateCategoryDto
    {
        [Required]
        public string Category { get; set; }
    }

    public class UpdateCategoryDto : CreateCategoryDto
    {
        public int CategoryId { get; set; }
    }
}
