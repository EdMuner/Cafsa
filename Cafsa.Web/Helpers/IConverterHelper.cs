using System.Threading.Tasks;
using Cafsa.Web.Data.Entities;
using Cafsa.Web.Models;

namespace Cafsa.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<Service> ToServiceAsync(ServiceViewModel model, bool isNew);
    }
}