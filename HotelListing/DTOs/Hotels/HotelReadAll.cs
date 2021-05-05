using HotelListing.DTOs.Countries;

namespace HotelListing.DTOs.Hotels
{
    public class HotelReadAll
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public int CountryId { get; set; }
        public CountryRead Country { get; set; }
    }
}
