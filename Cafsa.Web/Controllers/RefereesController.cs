using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cafsa.Web.Data;
using Cafsa.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Cafsa.Web.Models;
using Cafsa.Web.Helpers;

namespace Cafsa.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class RefereesController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;



        //Se le injecta el IUser helper para crear los User para crear los referees
        public RefereesController(
            DataContext datacontext,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper
          )
        {
            _dataContext = datacontext;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
        }

        // GET: Referees
        public IActionResult Index()
        {
            //return View(await _datacontext.Referees.ToListAsync());
            //No hay que materializar l alista con el Tolist()
            return View(_dataContext.Referees
                .Include(r => r.User)
                .Include(r => r.Activities)
                .ThenInclude(r => r.ActivityType)
                .Include(r => r.Services));
        }

        // GET: Referees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referee = await _dataContext.Referees
                .Include(r => r.User)
                .Include(o => o.Activities)
                .ThenInclude(o => o.ActivityType)
                .Include(o => o.Activities)
                .ThenInclude(a => a.ActivityImages)
                .Include(r => r.Services)
                .ThenInclude(c => c.Client)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (referee == null)
            {
                return NotFound();
            }

            return View(referee);
        }

        // GET: Referees/Create
        public IActionResult Create()
        {
            var view = new AddUserViewModel { RoleId = 2 };
            return View(view);          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await CreateUserAsync(model);
                if (user != null)
                {
                    var referee = new Referee
                    {
                        Services = new List<Service>(),
                        Activities = new List<Activity>(),
                        User = user

                    };

                    _dataContext.Referees.Add(referee);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction("Index");

                }

                ModelState.AddModelError(String.Empty, "User with this email already exist");
            }
            return View(model);
        }

        private async Task<User> CreateUserAsync(AddUserViewModel model)
        {
            var user = new User
            {
                Address = model.Address,
                Document = model.Document,
                Email = model.Username,
                FirstName = model.FirstName,          
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username
            };

            var result = await _userHelper.AddUserAsync(user, model.Password);

            if (result.Succeeded)
            {
                user = await _userHelper.GetUserByEmailAsync(model.Username);
                await _userHelper.AddUserToRoleAsync(user, "Referee");
                return user;
            }
            return null;
        }

        // GET: Referees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referee = await _dataContext.Referees
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (referee == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Address = referee.User.Address,
                Document = referee.User.Document,
                FirstName = referee.User.FirstName,
                Id = referee.Id,
                LastName = referee.User.LastName,
                PhoneNumber = referee.User.PhoneNumber

            };

            return View(model);
        }

        // POST: Referees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var referee = await _dataContext.Referees
                      .Include(o => o.User)
                      .FirstOrDefaultAsync(o => o.Id == model.Id);

                referee.User.Document = model.Document;
                referee.User.FirstName = model.FirstName;
                referee.User.LastName = model.LastName;
                referee.User.Address = model.Address;
                referee.User.PhoneNumber = model.PhoneNumber;

                await _userHelper.UpdateUserAsync(referee.User);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Referees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referee = await _dataContext.Referees
                .Include(r => r.User)
                .Include(r => r.Activities)         
                .FirstOrDefaultAsync(m => m.Id == id);
            if (referee == null)
            {
                return NotFound();
            }

            if (referee.Activities.Count != 0)
            {
                ModelState.AddModelError(string.Empty, "Referee can't be delete because it has activities.");
                return RedirectToAction(nameof(Index));
            }

            _dataContext.Referees.Remove(referee);
            await _dataContext.SaveChangesAsync();
            await _userHelper.DeleteUserAsync(referee.User.Email);
            return RedirectToAction(nameof(Index));
        }

     

        private bool RefereeExists(int id)
        {
            return _dataContext.Referees.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddActivity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referee = await _dataContext.Referees.FindAsync(id);
            if (referee == null)
            {
                return NotFound();
            }

            var model = new ActivityViewModel
            {
                RefereeId = referee.Id,
                ActivityTypes = _combosHelper.GetComboActivityTypes()

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddActivity(ActivityViewModel model)
        {
            if (ModelState.IsValid)
            {
                //creamos un nuevo metodo para transformar el modelo y se lo enviamos al ConverterHelper para que lo cambie.
                var activity = await _converterHelper.ToActivityAsync(model, true);
                //Creamos el sevicio en la DB
                _dataContext.Activities.Add(activity);
                //utilizamos el metodo para guardar los cambios
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"Details/{model.RefereeId}");

            }
            //carga nuevamente el combo para nuevamente escoger un tipo de actividad
            model.ActivityTypes = _combosHelper.GetComboActivityTypes();
            return View(model);
        }

        public async Task<IActionResult> EditActivity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _dataContext.Activities
                .Include(r => r.Referee)
                .Include(r => r.ActivityType)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (activity == null)
            {
                return NotFound();
            }
            
            var model = _converterHelper.ToActivityViewModel(activity);
            return View(model);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> EditActivity(ActivityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var activity = await _converterHelper.ToActivityAsync(model, false);
                _dataContext.Activities.Update(activity);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"Details/{model.RefereeId}");
            }
            return View(model);
        }

        public async Task<IActionResult> DetailsActivity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _dataContext.Activities
                .Include(r => r.Referee)
                .ThenInclude(r => r.User)
                .Include(r => r.Services)
                .ThenInclude(c => c.Client)
                .ThenInclude(c => c.User)
                .Include(r => r.ActivityType)
                .Include(a => a.ActivityImages)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        //aqui se recibe el Id de la propiedad que se le va a agregar la image
        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _dataContext.Activities.FindAsync(id.Value);
            if (activity == null)
            {
                return NotFound();
            }

            var model = new ActivityImageViewModel
            {
                Id = activity.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(ActivityImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                //se valida lo que envio IFormFile
                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile);
                }

                var activityImage = new ActivityImage
                {
                    ImageUrl = path,
                    Activity = await _dataContext.Activities.FindAsync(model.Id)
                };

                _dataContext.ActivityImages.Add(activityImage);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsActivity)}/{model.Id}");
            }

            return View(model);
        }

        public async Task<IActionResult> AddService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _dataContext.Activities
                .Include(p => p.Referee)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (activity == null)
            {
                return NotFound();
            }

            var model = new ServiceViewModel
            {
                RefereeId = activity.Referee.Id,
                ActivityId = activity.Id,
                Clients = _combosHelper.GetComboClients(),
                Price = activity.Price,
                StartDate = DateTime.Today,
            };

            model.Clients = _combosHelper.GetComboClients();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddService(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var service = await _converterHelper.ToServiceAsync(model, true);
                _dataContext.Services.Add(service);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsActivity)}/{model.ActivityId}");
            }

            model.Clients = _combosHelper.GetComboClients();
            return View(model);
        }

        public async Task<IActionResult> EditService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _dataContext.Services
                .Include(p => p.Referee)
                .Include(p => p.Client)
                .Include(p => p.Activity)              
                
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (service == null)
            {
                return NotFound();
            }

            return View(_converterHelper.ToServiceViewModel(service));
        }

        [HttpPost]
        public async Task<IActionResult> EditService(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var service = await _converterHelper.ToServiceAsync(model, false);
                _dataContext.Services.Update(service);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsActivity)}/{model.ActivityId}");
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityImage = await _dataContext.ActivityImages
                .Include(pi => pi.Activity)
                .FirstOrDefaultAsync(pi => pi.Id == id.Value);
            if (activityImage == null)
            {
                return NotFound();
            }

            _dataContext.ActivityImages.Remove(activityImage);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(DetailsActivity)}/{activityImage.Activity.Id}");
        }

        public async Task<IActionResult> DeleteService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _dataContext.Services
                .Include(c => c.Activity)
                .FirstOrDefaultAsync(c => c.Id == id.Value);
            if (service == null)
            {
                return NotFound();
            }

            _dataContext.Services.Remove(service);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(DetailsActivity)}/{service.Activity.Id}");
        }

        public async Task<IActionResult> DeleteActivity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _dataContext.Activities
                .Include(p => p.Referee)
                .Include(p => p.ActivityImages)
                .Include(p => p.Services)
                .FirstOrDefaultAsync(pi => pi.Id == id.Value);
            if (activity == null)
            {
                return NotFound();
            }
            if (activity.Services.Count != 0)
            {
                ModelState.AddModelError(string.Empty, "The Activity can't be deleted because it has Services.");
                return RedirectToAction($"{nameof(Details)}/{activity.Referee.Id}");
            }

            _dataContext.ActivityImages.RemoveRange(activity.ActivityImages);      
            _dataContext.Activities.Remove(activity);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{activity.Referee.Id}");
        }

        // se recibe el id del servicio
        public async Task<IActionResult> DetailsService(int? id)
        {
            //validamos si viene null y salimos
            if (id == null)
            {
                return NotFound();
            }

            //consultamos en la DB los servicios con ese id
            //se crea el objeto service con toda la información del servicio.
            var service = await _dataContext.Services
                //incluimos el Referee y el user del referee
                .Include(c => c.Referee)
                .ThenInclude(o => o.User)
                //ncluimos el client y el user del client
                .Include(c => c.Client)
                .ThenInclude(o => o.User)
                //incluimos la activity y el activityType
                .Include(c => c.Activity)
                .ThenInclude(p => p.ActivityType)
                .FirstOrDefaultAsync(pi => pi.Id == id.Value);
            if (service == null)
            {
                return NotFound();

            }
            //retornamos a la vista el servicio con toda la información que incluimos.
            return View(service);
        }


    }
}
