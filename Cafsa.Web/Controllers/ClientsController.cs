using Cafsa.Web.Data;
using Cafsa.Web.Data.Entities;
using Cafsa.Web.Helpers;
using Cafsa.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cafsa.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;

        public ClientsController(
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
            return View(_dataContext.Clients
                .Include(c => c.User)
                .Include(c => c.Services));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _dataContext.Clients
                .Include(c => c.User)
                .Include(c => c.Services)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        public IActionResult Create()
        {
            var view = new AddUserViewModel { RoleId = 1 };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.AddUser(model, "Client");
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    return View(model);
                }

                var client = new Client
                {
                    Services = new List<Service>(),
                    User = user,
                };

                _dataContext.Clients.Add(client);
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

            var client = await _dataContext.Clients
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (client == null)
            {
                return NotFound();
            }

            var view = new EditUserViewModel
            {
                Address = client.User.Address,
                Document = client.User.Document,
                FirstName = client.User.FirstName,
                Id = client.Id,
                LastName = client.User.LastName,
                PhoneNumber = client.User.PhoneNumber
            };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = await _dataContext.Clients
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.Id == model.Id);

                client.User.Document = model.Document;
                client.User.FirstName = model.FirstName;
                client.User.LastName = model.LastName;
                client.User.Address = model.Address;
                client.User.PhoneNumber = model.PhoneNumber;

                await _userHelper.UpdateUserAsync(client.User);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _dataContext.Clients
                .Include(r => r.User)
                .Include(r => r.Services)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            if (client.Services.Count != 0)
            {
                ModelState.AddModelError(string.Empty, "Client can't be delete because it has services.");
                return RedirectToAction(nameof(Index));
            }

            _dataContext.Clients.Remove(client);
            await _dataContext.SaveChangesAsync();
            await _userHelper.DeleteUserAsync(client.User.Email);
            return RedirectToAction(nameof(Index));
        }
    }
}
