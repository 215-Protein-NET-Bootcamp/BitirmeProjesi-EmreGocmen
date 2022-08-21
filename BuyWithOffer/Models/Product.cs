using System.ComponentModel.DataAnnotations;

namespace BuyWithOffer
{
    public class Product : CommonEntity
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} field max {1}, at least {2} chars", MinimumLength = 3)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "{0} field max {1}, at least {2} chars", MinimumLength = 10)]
        public string Explanation { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public int ColorId { get; set; }

        public int BrandId { get; set; }

        [Required]
        public int UsageStatusId { get; set; }

        public int PhotoId { get; set; }

        [Required]
        public int Price { get; set; }

        public bool hasPhoto { get; set; } = false;
        public bool isOfferable { get; set; } = false;
        public bool isSold { get; set; } = false;
    }
}
