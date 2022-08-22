using System.ComponentModel.DataAnnotations;

namespace BuyWithOffer
{
    public class LoginModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} field max {1}, at least {2} chars", MinimumLength = 8)]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [StringLength(20, ErrorMessage = "{0} field max {1}, at least {2} chars", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
