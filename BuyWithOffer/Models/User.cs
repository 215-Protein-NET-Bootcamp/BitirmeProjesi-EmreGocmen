using Microsoft.AspNetCore.Identity;

namespace BuyWithOffer
{
    public class User : IdentityUser
    {
        public new long? PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
