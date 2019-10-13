using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cafsa.Common.Models;
using Cafsa.Web.Data;
using Cafsa.Web.Data.Entities;

namespace Cafsa.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class RefereesController : ControllerBase
    {
        private readonly DataContext _dataContext;
       

        //se utiliza el post para traer por que el get es muy inseguro, la utilizacion de Get, Post, Put es una convención,
        // pero usted puede utilizar cualquiera para borrar, actualizar y crear.

        // se crea en el common en los modelos la clase Email Request para que este controlador reciba un response de Email,
        //por que en esta clase no se puede enviar caracterez especiales y el email tiene @

        public RefereesController(
             DataContext dataContext)
         
        {
            _dataContext = dataContext;
          
        }

        //el metodo se llama desde la api como esta GetRefereeByEmail y en el controlador es Async
        [HttpPost]
        [Route("GetRefereeByEmail")]
        public async Task<IActionResult> GetRefereeByEmailAsync(EmailRequest request)
        {
            //Se valida que el modelo sea valido
            if (!ModelState.IsValid)
            {
                //retorna error status 400
                return BadRequest();
            }
            //incluimos las tablas de las cuales vamos a extraer información.
            var referee = await _dataContext.Referees
                .Include(r => r.User)
                .Include(r => r.Activities)
                .ThenInclude(a => a.ActivityType)
                .Include(r => r.Activities)
                .ThenInclude(r => r.ActivityImages)
                .Include(r => r.Services)
                .ThenInclude(c => c.Client)
                .ThenInclude(c => c.User)
                //se conviernten a minusculas para validar que si encontro el referee con el email
                .FirstOrDefaultAsync(r => r.User.Email.ToLower() == request.Email.ToLower());

            if (referee == null)
            {
                return NotFound();
            }

            var response = new RefereeResponse
         
            {
                Id = referee.Id,
                Category = referee.User.Category,
                FirstName = referee.User.FirstName,
                LastName = referee.User.LastName,
                Address = referee.User.Address,
                Document = referee.User.Document,            
                Email = referee.User.Email,
                PhoneNumber = referee.User.PhoneNumber,
                Activities = referee.Activities?.Select(p => new ActivityResponse
                {
                    Address = p.Address,
                    Services = p.Services?.Select(s => new ServiceResponse
                    {
                        Id = s.Id,
                        IsActive = s.IsActive,
                        Client = ToClientResponse(s.Client),
                        Price = s.Price,
                        Remarks = s.Remarks,
                        StartDate = s.StartDate
                    }).ToList(),
                    Id = p.Id,
                    IsAvailable = p.IsAvailable,
                    Neighborhood = p.Neighborhood,
                    Price = p.Price,
                    ActivityImages = p.ActivityImages?.Select(pi => new ActivityImageResponse
                    {
                        Id = pi.Id,
                        ImageUrl = pi.ImageFullPath
                    }).ToList(),
                    ActivityType = p.ActivityType.Name,
                    Remarks = p.Remarks,
                }).ToList()
            };

            //para poder desserializar como un Json hay que crear una clase donde no tenga las referencias circulares.
            //se crea todas las clase en el commond, que son las encargadas de dar response a este controller
            return Ok(response);
        }

        private ClientResponse ToClientResponse(Client client)
        {
            return new ClientResponse
            {
                Id = client.Id,
                Address = client.User.Address,
                Document = client.User.Document,
                Email = client.User.Email,
                FirstName = client.User.FirstName,
                LastName = client.User.LastName,
                Category = client.User.Category,
                PhoneNumber = client.User.PhoneNumber
            };
        }
    }
}

