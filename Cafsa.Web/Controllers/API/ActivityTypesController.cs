using Cafsa.Web.Data;
using Cafsa.Web.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// get para traer informacion
// post envie y cree nuevos registros
// put para modificar registros y delete para borrar registros.

namespace Cafsa.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActivityTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public ActivityTypesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ActivityTypes
        [HttpGet]
        public IEnumerable<ActivityType> GetActivityTypes()
        {
            return _context.ActivityTypes.OrderBy(at => at.Name);
        }

    }
}