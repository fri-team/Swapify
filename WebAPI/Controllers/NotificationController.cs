using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Entities.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class NotificationController: BaseController
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public NotificationController(INotificationService notificationService, IUserService userService)
        {
            _notificationService = notificationService;
            _userService = userService;
        }

        [HttpPut("{notificationId}/{read}")]
        public async Task<IActionResult> UpdateNotificationReadState(Guid notificationId, bool read)
        {
            await _notificationService.UpdateNotificationReadState(notificationId, read);
            return Ok();
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetStudentNotifications(string email)
        {
            User user = await _userService.GetUserByEmailAsync(email);

            if (user.Student == null)
            {
                return Ok(new List<Notification>());
            }

            var notifications = await _notificationService.GetStudentNotifications(user.Student.Id);
            return Ok(notifications);
        }
    }
}
