using Microsoft.AspNetCore.Identity;

namespace HotelListing.Models
{
    /// <summary>
    /// Extending IdentityUser.
    /// </summary>
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
