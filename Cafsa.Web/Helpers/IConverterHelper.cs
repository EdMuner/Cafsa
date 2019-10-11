using System.Threading.Tasks;
using Cafsa.Web.Data.Entities;
using Cafsa.Web.Models;

namespace Cafsa.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<Activity> ToActivityAsync(ActivityViewModel model, bool isNew);


        // ya hay que hacer lo contrario recibir una actividad y convertirla a una viewModel para poderla editar.
        ActivityViewModel ToActivityViewModel(Activity activity);
      

    }
}