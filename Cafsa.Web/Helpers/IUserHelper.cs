using Cafsa.Web.Data.Entities;
using Cafsa.Web.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Cafsa.Web.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model); 

        Task<IdentityResult> UpdateUserAsync(User user);

        Task LogoutAsync();

        Task<bool> DeleteUserAsync(string email);

        Task<SignInResult> ValidatePasswordAsync(User user, string password);
    }
}
