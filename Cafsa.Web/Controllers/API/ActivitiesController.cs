using Cafsa.Common.Helpers;
using Cafsa.Common.Models;
using Cafsa.Web.Data;
using Cafsa.Web.Data.Entities;
using Cafsa.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Cafsa.Web.Controllers.API
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActivitiesController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;

        public ActivitiesController(
            DataContext dataContext,
            IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
        }

        [HttpPost]
        public async Task<IActionResult> PostActivity([FromBody] ActivityRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var referee = await _dataContext.Referees.FindAsync(request.RefereeId);
            if (referee == null)
            {
                return BadRequest("Not valid referee.");
            }

            var activityType = await _dataContext.ActivityTypes.FindAsync(request.ActivityTypeId);
            if (activityType == null)
            {
                return BadRequest("Not valid activity type.");
            }

            var activity = new Activity
            {
                Address = request.Address,
                IsAvailable = request.IsAvailable,
                Neighborhood = request.Neighborhood,
                Referee = referee,
                Price = request.Price,
                ActivityType = activityType,
                Remarks = request.Remarks,
            };

            _dataContext.Activities.Add(activity);
            await _dataContext.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPost]
        [Route("AddImageToActivity")]
        public async Task<IActionResult> AddImageToActivity([FromBody] ImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activity = await _dataContext.Activities.FindAsync(request.ActivityId);
            if (activity == null)
            {
                return BadRequest("Not valid activity.");
            }

            var imageUrl = string.Empty;
            if (request.ImageArray != null && request.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(request.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot\\images\\Activities";
                var fullPath = $"~/images/Activities/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    imageUrl = fullPath;
                }
            }

            var activityImage = new ActivityImage
            {
                ImageUrl = imageUrl,
                Activity = activity
            };

            _dataContext.ActivityImages.Add(activityImage);
            await _dataContext.SaveChangesAsync();
            return Ok(activityImage);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity([FromRoute] int id, [FromBody] ActivityRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Id)
            {
                return BadRequest();
            }

            var oldActivity = await _dataContext.Activities.FindAsync(request.Id);
            if (oldActivity == null)
            {
                return BadRequest("Activity doesn't exists.");
            }

            var activityType = await _dataContext.ActivityTypes.FindAsync(request.ActivityTypeId);
            if (activityType == null)
            {
                return BadRequest("Not valid activity type.");
            }

            oldActivity.Address = request.Address;
            oldActivity.IsAvailable = request.IsAvailable;
            oldActivity.Neighborhood = request.Neighborhood;
            oldActivity.Price = request.Price;
            oldActivity.ActivityType = activityType;
            oldActivity.Remarks = request.Remarks;

            _dataContext.Activities.Update(oldActivity);
            await _dataContext.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPost]
        [Route("DeleteImageToActivity")]
        public async Task<IActionResult> DeleteImageToActivity([FromBody] ImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activityImage = await _dataContext.ActivityImages.FindAsync(request.Id);
            if (activityImage == null)
            {
                return BadRequest("Activity image doesn't exist.");
            }

            _dataContext.ActivityImages.Remove(activityImage);
            await _dataContext.SaveChangesAsync();
            return Ok(activityImage);
        }
    }
}
