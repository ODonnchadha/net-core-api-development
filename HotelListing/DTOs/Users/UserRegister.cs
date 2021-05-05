using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs.Users
{
    public class UserRegister
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required()]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required()]
        [StringLength(15, ErrorMessage = "Your password is limited to fifteen characters")]
        public string Password { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
