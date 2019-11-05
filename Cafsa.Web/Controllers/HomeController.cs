using Cafsa.Web.Data;
using Cafsa.Web.Models;
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

        public HomeController(DataContext dataContext)
        {
            _dataContext = dataContext;
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

    }
}
