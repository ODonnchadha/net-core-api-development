using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs.Hotels
{
    public class HotelCreate
    {
        [Required()]
        [StringLength(maximumLength: 150, ErrorMessage = "Hotel name cannot exceed 50 characters in length")]
        public string Name { get; set; }

        [Required()]
        [StringLength(maximumLength: 250, ErrorMessage = "Hotel address cannot exceed 50 characters in length")]
        public string Address { get; set; }

        [Required()]
        [Range(1,5)]
        public double Rating { get; set; }

        [Required()]
        public int CountryId { get; set; }
    }
}
