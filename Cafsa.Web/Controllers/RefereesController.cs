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



        //Se le injecta el IUser helper para crear los User para crear los referees
        public RefereesController(
            DataContext datacontext,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper
          )
        {
            _dataContext = datacontext;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        // GET: Referees
        public IActionResult Index()
        {
            //return View(await _datacontext.Referees.ToListAsync());
            //No hay que materializar l alista con el Tolist()
            return View(_dataContext.Referees
                .Include(r => r.User)
                .Include(r => r.Services)
                .Include(r => r.Contracts));//No materializa la lista y le digo que me incluya los usuarios, en otras palabras busqueme los referees e inclullame lo usuarios
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
                .Include(r => r.Contracts)
                .ThenInclude(c => c.Client)
                .ThenInclude(r => r.User)
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
                        Contracts = new List<Contract>(),
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

            var owner = await _dataContext.Referees
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (owner == null)
            {
                return NotFound();
            }

            var view = new EditUserViewModel
            {
                Address = owner.User.Address,
                Document = owner.User.Document,
                FirstName = owner.User.FirstName,
                Id = owner.Id,
                LastName = owner.User.LastName,
                Category = owner.User.Category,
                PhoneNumber = owner.User.PhoneNumber

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

        public async Task<IActionResult> AddService(int id)
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
                ServiceTypes =  _combosHelper.GetComboServiceTypes()
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
            return View(model);
        }     
    }  
}
