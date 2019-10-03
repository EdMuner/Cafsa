using Cafsa.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Cafsa.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboRefereeTypes()
        {
            var list = _dataContext.RefereeTypes.Select(rt => new SelectListItem
            {
                Text = rt.Name,
                Value = $"{ rt.Id}"
            })
                .OrderBy(rt => rt.Text)
                .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Referee Category)",
                Value = "0"
            });
            return list;
        }
    }
}
