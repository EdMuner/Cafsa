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

     
        //Metodo para crear el combo de service type
        public IEnumerable<SelectListItem> GetComboActivityTypes()
        {

            //list campo text y value
            var list = _dataContext.ActivityTypes.Select(st => new SelectListItem
            {
                Text = st.Name,
                //se comvierte a String con interpolacion
                Value = $"{st.Id}"
            })
                .OrderBy(st => st.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Activity Type"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboClients()
        {
            var list = _dataContext.Clients.Select(c => new SelectListItem
            {
                Text = c.User.FullNameWithDocument,
                Value = $"{c.Id}"
            })
              .OrderBy(c => c.Text)
              .ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a Client...)",
                Value = "0"
            });
            return list;
        }
    }
}
