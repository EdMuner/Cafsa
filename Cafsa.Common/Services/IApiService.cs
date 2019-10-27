using System.Threading.Tasks;
using Cafsa.Common.Models;

namespace Cafsa.Common.Services
{
    public interface IApiService
    {
        Task<Response<RefereeResponse>> GetRefereeByEmailAsync(
            string urlBase, 
            string servicePrefix, 
            string controller, 
            string tokenType, 
            string accessToken, 
            string email);

        Task<Response<TokenResponse>> GetTokenAsync(
            string urlBase, 
            string servicePrefix, 
            string controller, 
            TokenRequest request);

        Task<bool> CheckConnectionAsync(string url);
    }
}