using Cafsa.Web.Data;
using Cafsa.Web.Helpers;
using Cafsa.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Cafsa.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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


    }
}
