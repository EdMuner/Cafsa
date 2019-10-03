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



        //Se le injecta el IUser helper para crear los User para crear los referees
        public RefereesController(
            DataContext datacontext,
            IUserHelper userHelper
          )
        {
            _dataContext = datacontext;
            _userHelper = userHelper;

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


        //Metodo que crea el referee      
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
                        // le agrega al referee nuevo una lista de contratos para que al crearlo el campo no este vacio
                        Contracts = new List<Contract>(),
                        // le agrega al referee nuevo una lista de services para que al crearlo el campo no este vacio
                        Services = new List<Service>(),

                        User = user

                    };
                    //crea el referee en base de datos, guarda cambios y lo redirecciona al index y se puede loguear.
                    _dataContext.Referees.Add(referee);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                };
                ModelState.AddModelError(string.Empty, "The user already exists in the corporation");
            };
            return View(model);
        }

        //metodo para cvrear el usuario y retornarlo
        private async Task<User> CreateUserAsync(AddUserViewModel model)
        {
            var user = new User
            {
                Document = model.Document,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Username,
                Address = model.Address,
                Phone = model.Phone,
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

            var referee = await _dataContext.Referees.FindAsync(id);
            if (referee == null)
            {
                return NotFound();
            }
            return View(referee);
        }

        // POST: Referees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Category")] Referee referee)
        {
            if (id != referee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataContext.Update(referee);
                    await _dataContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefereeExists(referee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(referee);
        }

        // GET: Referees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referee = await _dataContext.Referees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (referee == null)
            {
                return NotFound();
            }

            return View(referee);
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

    }
}
