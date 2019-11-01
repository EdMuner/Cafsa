using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cafsa.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboActivityTypes();

        IEnumerable<SelectListItem> GetComboClients();

        IEnumerable<SelectListItem> GetComboRoles();

    }
}