using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cafsa.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboServiceTypes();

        IEnumerable<SelectListItem> GetComboClients();
    }
}