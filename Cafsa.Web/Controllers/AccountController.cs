using Cafsa.Web.Data;
using Cafsa.Web.Data.Entities;
using Cafsa.Web.Helpers;
using Cafsa.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cafsa.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly ICombosHelper _combosHelper;
        private readonly IMailHelper _mailHelper;

        public AccountController(
            DataContext dataContext,
            IUserHelper userHelper,
            IConfiguration configuration,
            ICombosHelper combosHelper,
            IMailHelper mailHelper
            )

        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _configuration = configuration;
            _combosHelper = combosHelper;
            _mailHelper = mailHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)

                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "User or password incorrect.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,

                            expires: DateTime.UtcNow.AddMonths(6),

                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created(string.Empty, results);
                    }
                }
            }

            return BadRequest();
        }
        public IActionResult NotAuthorized()
        {
            return View();
        }
        public IActionResult Register()
        {
            var model = new AddUserViewModel
            {
                Roles = _combosHelper.GetComboRoles()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = "Referee";
                if (model.RoleId == 1)
                {
                    role = "Client";
                }

                var user = await _userHelper.AddUser(model, role);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "This email is already used.");
                    return View(model);
                }

                if (model.RoleId == 1)
                {
                    var client = new Client
                    {
                        Services = new List<Service>(),
                        User = user
                    };

                    _dataContext.Clients.Add(client);              
                }
                else
                {
                    var referee = new Referee
                    {
                        Services = new List<Service>(),
                        Activities = new List<Activity>(),
                        User = user
                    };

                    _dataContext.Referees.Add(referee);
                }

                await _dataContext.SaveChangesAsync();



                //validacion por correo del registro de un nuevo referee or client
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
                     $"       <li> Arbitros.</li>" +
                     $"        <li> Anotadores.</li>" +
                     $"        <li> Cronometristas.</li>" +
                     $"        <li> Instructores.</li>" +
                     $"        <li> Coordinadores.</li>" +
                     $"      </ul>" +
                     $"  <div style = 'width: 100%;margin:20px 0; display: inline-block;text-align: center'>" +
                     $"  </div>" +
                     $"  <div style = 'width: 100%; text-align: center'>" +
                     $"    <a style ='text-decoration: none; border-radius: 5px; padding: 11px 23px; color: white; background-color: #3498db' href = \"{tokenLink}\">Confirm Email</a>" +
                     $"</tr>" +
                     $"</table>");
                ViewBag.Message = "The instructions to allow your user has been sent to email.";
                return View(model);






            }
            model.Roles = _combosHelper.GetComboRoles();
            return View(model);
        }
        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            var view = new EditUserViewModel
            {
                Address = user.Address,
                Document = user.Document,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };

            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                user.Document = model.Document;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // capturamos el usuario logueado para poder cambiarle el password
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }

    }
}