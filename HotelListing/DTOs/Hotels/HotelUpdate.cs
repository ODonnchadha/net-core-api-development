using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs.Hotels
{
    public class HotelUpdate
    {
        [StringLength(maximumLength: 150, ErrorMessage = "Hotel name cannot exceed 50 characters in length")]
        public string Name { get; set; }

        [StringLength(maximumLength: 250, ErrorMessage = "Hotel address cannot exceed 50 characters in length")]
        public string Address { get; set; }

        [Range(1, 5)]
        public double Rating { get; set; }

        public int CountryId { get; set; }
    }
}
