using Cafsa.Web.Data;
using Cafsa.Web.Data.Entities;
using Cafsa.Web.Helpers;
using Cafsa.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;



namespace Cafsa.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly Activity sol;

        public HomeController(
            DataContext dataContext,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
           
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        public IActionResult SearchActivities()  
        {
            return View(_dataContext.Activities
                .Include(p => p.ActivityType)
                .Include(p => p.ActivityImages)
                .Where(p => p.IsAvailable));
        }

        public async Task<IActionResult> DetailsActivity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _dataContext.Activities
                .Include(o => o.ActivityType)
                .Include(p => p.ActivityImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        [Authorize(Roles = "Referee")]
        public async Task<IActionResult> MyActivities()
        {
            var referee = await _dataContext.Referees
                .Include(o => o.User)
                .Include(o => o.Services)
                .Include(o => o.Activities)
                .ThenInclude(p => p.ActivityType)
                .Include(o => o.Activities)
                .ThenInclude(p => p.ActivityImages)
                .FirstOrDefaultAsync(o => o.User.UserName.ToLower().Equals(User.Identity.Name.ToLower()));
            if (referee == null)
            {
                return NotFound();
            }

            return View(referee);
        }
        [Authorize(Roles = "Referee")]
        public async Task<IActionResult> AddActivity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referee = await _dataContext.Referees.FindAsync(id.Value);
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
                var activity = await _converterHelper.ToActivityAsync(model, true);
                _dataContext.Activities.Add(activity);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(MyActivities));
            }

            model.ActivityTypes = _combosHelper.GetComboActivityTypes();
            return View(model);
        }

        [Authorize(Roles = "Referee")]
        public async Task<IActionResult> EditActivity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _dataContext.Activities
                .Include(p => p.Referee)
                .Include(p => p.ActivityType)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
            if (activity == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToActivityViewModel(activity);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditActivity(ActivityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var activity = await _converterHelper.ToActivityAsync(model, false);
                _dataContext.Activities.Update(activity);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(MyActivities));
            }

            model.ActivityTypes = _combosHelper.GetComboActivityTypes();
            return View(model);
        }

        [Authorize(Roles = "Referee")]
        public async Task<IActionResult> DetailsActivityReferee(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _dataContext.Activities
                .Include(o => o.Referee)
                .ThenInclude(o => o.User)
                .Include(o => o.Services)
                .ThenInclude(c => c.Client)
                .ThenInclude(l => l.User)
                .Include(o => o.ActivityType)
                .Include(p => p.ActivityImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        [Authorize(Roles = "Referee")]
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

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Activities",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Activities/{file}";
                }

                var activityImage = new ActivityImage
                {
                    ImageUrl = path,
                    Activity = await _dataContext.Activities.FindAsync(model.Id)
                };

                _dataContext.ActivityImages.Add(activityImage);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsActivityReferee)}/{model.Id}");
            }

            return View(model);
        }


        [Authorize(Roles = "Referee")]
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
            return RedirectToAction($"{nameof(DetailsActivityReferee)}/{activityImage.Activity.Id}");
        }

        [Authorize(Roles = "Referee")]
        public async Task<IActionResult> DeleteActivity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _dataContext.Activities
                .Include(p => p.Referee)
                .Include(p => p.Services)
                .FirstOrDefaultAsync(pi => pi.Id == id.Value);
            if (activity == null)
            {
                return NotFound();
            }

            if (activity.Services?.Count > 0)
            {
                return RedirectToAction(nameof(MyActivities));
            }


            _dataContext.Activities.Remove(activity);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(MyActivities));
        }

        [Authorize(Roles = "Referee, Client")]
        public IActionResult MyServices()
        {
            return View(_dataContext.Services
                .Include(c => c.Referee)
                .ThenInclude(o => o.User)
                .Include(c => c.Client)
                .ThenInclude(l => l.User)
                .Include(c => c.Activity)
                .ThenInclude(p => p.ActivityType)
                .Where(c => c.Referee.User.UserName.ToLower().Equals(User.Identity.Name.ToLower()) ||
                            c.Client.User.UserName.ToLower().Equals(User.Identity.Name.ToLower())));
        }

        [Authorize(Roles = "Referee, Client")]
        public async Task<IActionResult> DetailsService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _dataContext.Services
                .Include(c => c.Referee)
                .ThenInclude(o => o.User)
                .Include(c => c.Client)
                .ThenInclude(l => l.User)
                .Include(c => c.Activity)
                .ThenInclude(p => p.ActivityType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }


    }
}
