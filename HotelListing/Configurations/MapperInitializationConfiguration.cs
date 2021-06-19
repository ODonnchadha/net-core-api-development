using AutoMapper;
using HotelListing.DTOs.Countries;
using HotelListing.DTOs.Hotels;
using HotelListing.DTOs.Users;
using HotelListing.Entities;
using HotelListing.Models;

namespace HotelListing.Configurations
{
    public class MapperInitializationConfiguration : Profile
    {
        public MapperInitializationConfiguration()
        {
            CreateMap<Country, CountryCreate>().ReverseMap();
            CreateMap<Country, CountryRead>().ReverseMap();
            CreateMap<Country, CountryReadAll>().ReverseMap();
            CreateMap<Country, CountryUpdate>().ReverseMap();

            CreateMap<Hotel, HotelCreate>().ReverseMap();
            CreateMap<Hotel, HotelRead>().ReverseMap();
            CreateMap<Hotel, HotelReadAll>().ReverseMap();
            CreateMap<Hotel, HotelUpdate>().ReverseMap();

            CreateMap<User, UserRegister>().ReverseMap();
            CreateMap<User, UserLogin>().ReverseMap();
        }
    }
}
