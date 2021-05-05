using HotelListing.DTOs.Users;
using System.Threading.Tasks;

namespace HotelListing.Interfaces.Managers
{
    public interface IAuthManager
    {
        string CreateToken();
        Task<bool> ValidateUserAsync(UserLogin userLogin);
    }
}
