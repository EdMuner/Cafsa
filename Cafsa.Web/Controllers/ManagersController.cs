using Cafsa.Web.Data;
using Cafsa.Web.Data.Entities;
using Cafsa.Web.Helpers;
using Cafsa.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cafsa.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagersController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;

        public ManagersController(
            DataContext dataContext,
            IUserHelper userHelper,
            IMailHelper mailHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
        }

        public IActionResult Index()
        {
            return View(_dataContext.Managers.Include(m => m.User));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _dataContext.Managers
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        public IActionResult Create()
        {
            return View(new AddUserViewModel { RoleId = 3 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.AddUser(model, "Manager");
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    return View(model);
                }

                var manager = new Manager { User = user };

                _dataContext.Managers.Add(manager);
                await _dataContext.SaveChangesAsync();

                var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                var tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                _mailHelper.SendMail(model.Username, "Cafsa Email confirmation",
                  $"<table style = 'max-width: 600px; padding: 10px; margin:0 auto; border-collapse: collapse;'>" +
                  $"  <tr>" +
                  $"    <td style = 'background-color: #34495e; text-align: center; padding: 0'>" +
                  $"  </td>" +
                  $"  </tr>" +
                  $"  <tr>" +
                  $"  <td style = 'padding: 0'>" +
                  $"  </td>" +
                  $"</tr>" +
                  $"<tr>" +
                  $" <td style = 'background-color: #ecf0f1'>" +
                  $"      <div style = 'color: #34495e; margin: 4% 10% 2%; text-align: justify;font-family: sans-serif'>" +
                  $"            <h1 style = 'color: #e67e22; margin: 0 0 7px' > Bienvenido </h1>" +
                  $"                    <p style = 'margin: 2px; font-size: 15px'>" +
                  $"                     La Corporacion arbitral de Futbol de Salon le da una cordial bienvenida," +
                  $"                     Formamos Arbitros en capacidad de juzgar eventos locales nacionales e internacionales .....<br>" +
                  $"                      Entre los servicios tenemos:</p>" +
                  $"      <ul style = 'font-size: 15px;  margin: 10px 0'>" +
                  $"       <li> Arbitros de Primera.</li>" +
                  $"        <li> Arbitros de Segunda Categoria.</li>" +
                  $"        <li> Anotadores.</li>" +
                  $"        <li> Cronometristas.</li>" +
                  $"        <li> Instructores.</li>" +
                  $"      </ul>" +
                  $"  <div style = 'width: 100%;margin:20px 0; display: inline-block;text-align: center'>" +
                  $"  </div>" +
                  $"  <div style = 'width: 100%; text-align: center'>" +
                  $"    <a style ='text-decoration: none; border-radius: 5px; padding: 11px 23px; color: white; background-color: #3498db' href = \"{tokenLink}\">Confirm Email</a>" +
                  $"</tr>" +
                  $"</table>");

                return RedirectToAction("Index");

            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _dataContext.Managers
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null)
            {
                return NotFound();
            }

            var view = new EditUserViewModel
            {
                Address = manager.User.Address,
                Document = manager.User.Document,
                FirstName = manager.User.FirstName,
                Id = manager.Id,
                LastName = manager.User.LastName,
                PhoneNumber = manager.User.PhoneNumber
            };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel view)
        {
            if (ModelState.IsValid)
            {
                var manager = await _dataContext.Managers
                    .Include(m => m.User)
                    .FirstOrDefaultAsync(o => o.Id == view.Id);

                manager.User.Document = view.Document;
                manager.User.FirstName = view.FirstName;
                manager.User.LastName = view.LastName;
                manager.User.Address = view.Address;
                manager.User.PhoneNumber = view.PhoneNumber;

                await _userHelper.UpdateUserAsync(manager.User);
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _dataContext.Managers
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null)
            {
                return NotFound();
            }

            _dataContext.Managers.Remove(manager);
            await _dataContext.SaveChangesAsync();
            await _userHelper.DeleteUserAsync(manager.User.Email);
            return RedirectToAction(nameof(Index));
        }
    }
}