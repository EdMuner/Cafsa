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

        public IEnumerable<SelectListItem> GetComboClients()
        {
            //list campo text y value
            var list = _dataContext.Clients.Select(cl => new SelectListItem
            {
                 Text = cl.User.FullNameWithDocument,
                //se comvierte a String con interpolacion
                Value = $"{cl.Id}"
            })
                .OrderBy(cl => cl.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Client..."
            });

            return list;
        }

        //Metodo para crear el combo de service type
        public IEnumerable<SelectListItem> GetComboServiceTypes()
        {

            //list campo text y value
            var list = _dataContext.ServiceTypes.Select(st => new SelectListItem
            {
                Text = st.Name,
                //se comvierte a String con interpolacion
                Value = $"{st.Id}"
            })
                .OrderBy(st => st.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a Service Type"
            });

            return list;
        }
    }
}
