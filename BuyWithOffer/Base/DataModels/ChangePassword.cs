using System.ComponentModel.DataAnnotations;

namespace BuyWithOffer
{
    public class ChangePassword
    {
        [Required]
        [StringLength(20, ErrorMessage = "{0} field max {1}, at least {2} chars", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }


        [Required]
        [StringLength(20, ErrorMessage = "{0} field max {1}, at least {2} chars", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
