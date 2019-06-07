using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.NotificationModels;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class NotificationController: BaseController
    {
        [AllowAnonymous]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserNotifications(string userId)
        {
            var notifications = new List<Notification>
            {
                new Notification()
                {
                    NotificationId = Guid.NewGuid(),
                    Type = "1",
                    Text = "ahoj",
                    CreatedAt = DateTime.Now,
                    Read = false                    
                },
                new Notification()
                {
                    NotificationId = Guid.NewGuid(),
                    Type = "1",
                    Text = "ahoj1",
                    CreatedAt = DateTime.Now,
                    Read = false
                },
                new Notification()
                {
                    NotificationId = Guid.NewGuid(),
                    Type = "1",
                    Text = "ahoj2",
                    CreatedAt = DateTime.Now,
                    Read = false
                },
            };

            return Ok(notifications);
        }
    }
}
