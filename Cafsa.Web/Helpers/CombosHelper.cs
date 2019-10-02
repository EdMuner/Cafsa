using Cafsa.Web.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafsa.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public IEnumerable<SelectListItem> GetComboCategoryTypes()
        {
            var list = _dataContext.RefereeTypes.Select(ct => new SelectListItem
            {
                Text = ct.Name,
                Value = $"{ct.Id}"
            })
                .OrderBy(ct => ct.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Category Type...)",
                Value = "0"
            });
            return list;
        }
    }
}
