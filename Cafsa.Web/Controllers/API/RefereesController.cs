using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cafsa.Common.Models;
using Cafsa.Web.Data;
using Cafsa.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Cafsa.Web.Helpers;
using System;

namespace Cafsa.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RefereesController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public RefereesController(
            DataContext dataContext,
            IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }

        [HttpPost]
        [Route("GetRefereeByEmail")]
        public async Task<IActionResult> GetReferee(EmailRequest emailRequest)
        {
            try
            {
                var user = await _userHelper.GetUserByEmailAsync(emailRequest.Email);
                if (user == null)
                {
                    return BadRequest("User not found.");
                }

                if (await _userHelper.IsUserInRoleAsync(user, "Referee"))
                {
                    return await GetRefereeAsync(emailRequest);
                }
                else
                {
                    return await GetClientAsync(emailRequest);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private async Task<IActionResult> GetClientAsync(EmailRequest emailRequest)
        {
            var client = await _dataContext.Clients
                .Include(o => o.User)
                .Include(o => o.Services)
                .ThenInclude(c => c.Referee)
                .ThenInclude(o => o.User)
                .FirstOrDefaultAsync(o => o.User.UserName.ToLower().Equals(emailRequest.Email.ToLower()));

            var activities = await _dataContext.Activities
                .Include(p => p.ActivityType)
                .Include(p => p.ActivityImages)
                .Where(p => p.IsAvailable)
                .ToListAsync();

            var response = new RefereeResponse
            {
                RoleId = 2,
                Id = client.Id,
                FirstName = client.User.FirstName,
                LastName = client.User.LastName,
                Address = client.User.Address,
                Document = client.User.Document,
                Email = client.User.Email,
                PhoneNumber = client.User.PhoneNumber,
                Activities = activities?.Select(p => new ActivityResponse
                {
                    Address = p.Address,         
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
                }).ToList(),
                Services = client.Services?.Select(c => new ServiceResponse
                {
                    Id = c.Id,
                    IsActive = c.IsActive,
                    Price = c.Price,
                    Remarks = c.Remarks,
                    StartDate = c.StartDate
                }).ToList()
            };

            return Ok(response);
        }

        private async Task<IActionResult> GetRefereeAsync(EmailRequest emailRequest)
        {
            var referee = await _dataContext.Referees
                .Include(o => o.User)
                .Include(o => o.Activities)
                .ThenInclude(p => p.ActivityType)
                .Include(o => o.Activities)
                .ThenInclude(p => p.ActivityImages)
                .Include(o => o.Services)
                .ThenInclude(c => c.Client)
                .ThenInclude(l => l.User)
                .FirstOrDefaultAsync(o => o.User.UserName.ToLower().Equals(emailRequest.Email.ToLower()));

            var response = new RefereeResponse
            {
                RoleId = 1,
                Id = referee.Id,
                FirstName = referee.User.FirstName,
                LastName = referee.User.LastName,
                Address = referee.User.Address,
                Document = referee.User.Document,
                Email = referee.User.Email,
                PhoneNumber = referee.User.PhoneNumber,
                Activities = referee.Activities?.Select(p => new ActivityResponse
                {
                    Address = p.Address,
                    Services = p.Services?.Select(c => new ServiceResponse
                    {
                        Id = c.Id,
                        IsActive = c.IsActive,
                        Client = ToClientsResponse(c.Client),
                        Price = c.Price,
                        Remarks = c.Remarks,
                        StartDate = c.StartDate
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
                }).ToList(),
                Services = referee.Services?.Select(c => new ServiceResponse
                {
                    Id = c.Id,
                    IsActive = c.IsActive,
                    Price = c.Price,
                    Remarks = c.Remarks,
                    StartDate = c.StartDate
                }).ToList()
            };

            return Ok(response);
        }

        private ClientResponse ToClientsResponse(Client client)
        {
            return new ClientResponse
            {
                Address = client.User.Address,
                Document = client.User.Document,
                Email = client.User.Email,
                FirstName = client.User.FirstName,
                LastName = client.User.LastName,
                PhoneNumber = client.User.PhoneNumber
            };
        }
    }
}



