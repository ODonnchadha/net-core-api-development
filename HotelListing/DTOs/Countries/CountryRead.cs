using HotelListing.DTOs.Hotels;
using System.Collections.Generic;

namespace HotelListing.DTOs.Countries
{
    public class CountryRead
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public IList<HotelRead> Hotels { get; set; }
    }
}
