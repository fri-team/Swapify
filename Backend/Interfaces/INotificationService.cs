using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Notifications;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface INotificationService
    {
        Task UpdateNotificationReadState(Guid notificationId, bool read);

        /// <summary>
        /// Get all notifications where user with studentId is recipient.
        /// </summary>
        /// <param name="studentId">Notification's recipientId.</param>
        /// <returns></returns>
        Task<IEnumerable<Notification>> GetStudentNotifications(Guid studentId);
        Task AddNotification(Notification notification);
    }
}
