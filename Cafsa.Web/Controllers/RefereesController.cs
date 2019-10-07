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
                .Include(r => r.Services)
                .ThenInclude(s => s.ServiceImages)                  
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
            return View();
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
                Category = model.Category,
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

            var view = new EditUserViewModel
            {
                Address = referee.User.Address,
                Document = referee.User.Document,
                FirstName = referee.User.FirstName,
                Id = referee.Id,
                LastName = referee.User.LastName,
                Category = referee.User.Category,
                PhoneNumber = referee.User.PhoneNumber

            };

            return View(view);
        }

        // POST: Referees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel view)
        {
            if (ModelState.IsValid)
            {
                var referee = await _dataContext.Referees
                      .Include(o => o.User)
                      .FirstOrDefaultAsync(o => o.Id == view.Id);

                referee.User.Document = view.Document;
                referee.User.FirstName = view.FirstName;
                referee.User.LastName = view.LastName;
                referee.User.Category = view.Category;
                referee.User.Address = view.Address;
                referee.User.PhoneNumber = view.PhoneNumber;

                await _userHelper.UpdateUserAsync(referee.User);
                return RedirectToAction(nameof(Index));
            }
            return View(view);
        }

        // GET: Referees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referee = await _dataContext.Referees
                .Include(o => o.User)
                .Include(o => o.Services)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (referee == null)
            {
                return NotFound();
            }

            if (referee.Services.Count != 0)
            {
                ModelState.AddModelError(string.Empty, "Owner can't be delete because it has properties.");
                return RedirectToAction(nameof(Index));
            }

            _dataContext.Referees.Remove(referee);
            await _dataContext.SaveChangesAsync();
            await _userHelper.DeleteUserAsync(referee.User.Email);
            return RedirectToAction(nameof(Index));
        }

        // POST: Referees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var referee = await _dataContext.Referees.FindAsync(id);
            _dataContext.Referees.Remove(referee);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RefereeExists(int id)
        {
            return _dataContext.Referees.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddService(int? id)
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

            var model = new ServiceViewModel
            {
                RefereeId = referee.Id,
                ServiceTypes =  _combosHelper.GetComboServiceTypes(),
                Clients = _combosHelper.GetComboClients()
                
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddService(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                //creamos un nuevo metodo para transformar el modelo y se lo enviamos al ConverterHelper para que lo cambie.
                var service = await _converterHelper.ToServiceAsync(model, true);
                //Creamos el sevicio en la DB
                _dataContext.Services.Add(service);
                //utilizamos el metodo para guardar los cambios
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"Details/{model.RefereeId}");

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
                .Include(c => c.Client)
                .Include(p => p.ServiceType)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToServiceViewModel(service);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditService(ServiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var service = await _converterHelper.ToServiceAsync(model, false);
                _dataContext.Services.Update(service);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"Details/{model.RefereeId}");
            }
            return View(model);
        }

        public async Task<IActionResult> DetailsService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _dataContext.Services
                .Include(o => o.Referee)
                .ThenInclude(o => o.User) 
                .Include(o => o.Client)
                .ThenInclude(o => o.Contracts)
                .Include(o => o.ServiceType)
                .Include(p => p.ServiceImages)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        //aqui se recibe el Id de la propiedad que se le va a agregar la image
        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _dataContext.Services.FindAsync(id.Value);
            if (service == null)
            {
                return NotFound();
            }

            var model = new ServiceImageViewModel
            {
                Id = service.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(ServiceImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                //se valida lo que envio IFormFile
                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile);
                }

                var serviceImage = new ServiceImage
                {
                    ImageUrl = path,
                    Service = await _dataContext.Services.FindAsync(model.Id)
                };

                _dataContext.ServiceImages.Add(serviceImage);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsService)}/{model.Id}");
            }

            return View(model);
        }

       


    }
}
