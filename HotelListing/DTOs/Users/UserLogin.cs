using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs.Users
{
    public class UserLogin
    {
        [Required()]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required()]
        [StringLength(15, ErrorMessage = "Your password is limited to fifteen characters")]
        public string Password { get; set; }
    }
}
