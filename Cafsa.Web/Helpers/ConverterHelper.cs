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
        public async Task<Activity> ToActivityAsync(ActivityViewModel model, bool isNew)
        {
            return new Activity
            {
                //Igualacion de propiedades
                Address = model.Address,
                //Si el servicio es nuevo se marca como 0, y si no que le ponemos el Id del modelo
                Services = isNew ? new List<Service>() : model.Services,
                Id = isNew ? 0 : model.Id,
                IsAvailable = model.IsAvailable,
                Neighborhood = model.Neighborhood,
                Referee = await _dataContext.Referees.FindAsync(model.RefereeId),
                Price = model.Price,
                ActivityImages = isNew ? new List<ActivityImage>() : model.ActivityImages,
                ActivityType = await _dataContext.ActivityTypes.FindAsync(model.ActivityTypeId),
                Remarks = model.Remarks,
            };
        }

        public ActivityViewModel ToActivityViewModel(Activity activity)
        {
            return new ActivityViewModel
            {
                Address = activity.Address,
                Services = activity.Services,
                Id = activity.Id,
                IsAvailable = activity.IsAvailable,
                Neighborhood = activity.Neighborhood,
                Referee = activity.Referee,
                Price = activity.Price,
                ActivityImages = activity.ActivityImages,
                ActivityType = activity.ActivityType,
                Remarks = activity.Remarks,
                RefereeId = activity.Referee.Id,
                ActivityTypeId = activity.ActivityType.Id,
                ActivityTypes = _combosHelper.GetComboActivityTypes(),
            };
        }

     
      
    }
}
