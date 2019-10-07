using Cafsa.Web.Data;
using Cafsa.Web.Data.Entities;
using Cafsa.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cafsa.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _dataContext;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(
            DataContext dataContext,
            ICombosHelper combosHelper)
        {
            _dataContext = dataContext;
            _combosHelper = combosHelper;
        }

        //metodo que recibe una ServiceViewModel y devuelve un Service
        public async Task<Service> ToServiceAsync(ServiceViewModel model, bool isNew)
        {
            return new Service
            {
                //Igualacion de propiedades
                Address = model.Address,
                //Si el servicio es nuevo se marca como 0, y si no que le ponemos el Id del modelo
                Id = isNew ? 0 : model.Id,
                Neighborhood = model.Neighborhood,
                Referee = await _dataContext.Referees.FindAsync(model.RefereeId),
                Price = model.Price,
                ServiceImages = isNew ? new List<ServiceImage>() : model.ServiceImages,
                ServiceType = await _dataContext.ServiceTypes.FindAsync(model.ServiceTypeId),
                Client = await _dataContext.Clients.FindAsync(model.ClientId),
                Remarks = model.Remarks,
                StartDate = model.StartDateLocal


            };
        }

        public ServiceViewModel ToServiceViewModel(Service service)
        {
            return new ServiceViewModel
            {
                Address = service.Address,                                    
                Id = service.Id,
                Neighborhood = service.Neighborhood,
                Referee = service.Referee,
                Price = service.Price,
                ServiceImages = service.ServiceImages,
                ServiceType = service.ServiceType,
                Remarks = service.Remarks,
                StartDate = service.StartDateLocal,
                RefereeId = service.Referee.Id,
                ServiceTypeId = service.ServiceType.Id,
                ServiceTypes = _combosHelper.GetComboServiceTypes(),
                Clients = _combosHelper.GetComboClients(),
            };
        }
    }
}
