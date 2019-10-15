using System.Threading.Tasks;
using Cafsa.Common.Models;

namespace Cafsa.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetRefereeByEmail(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, string email);
        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);
    }
}