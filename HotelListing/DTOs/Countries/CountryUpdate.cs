using System.ComponentModel.DataAnnotations;

namespace HotelListing.DTOs.Countries
{
    public class CountryUpdate
    {
        [StringLength(maximumLength: 50, ErrorMessage = "Country name cannot exceed 50 characters in length")]
        public string Name { get; set; }

        [StringLength(maximumLength: 2, ErrorMessage = "Short country name cannot exceed 2 characters in length")]
        public string ShortName { get; set; }
    }
}
