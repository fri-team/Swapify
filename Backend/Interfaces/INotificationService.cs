using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities;

namespace FRITeam.Swapify.Backend.Interfaces
{
    public interface INotificationService
    {
        Task UpdateNotificationReadState(Guid notificationId, bool read);

        /// <summary>
        /// Get all notifications where user with userId is recipient.
        /// </summary>
        /// <param name="userId">Notification's recipientId.</param>
        /// <returns></returns>
        Task<IEnumerable<Notification>> GetUserNotifications(Guid userId);
        Task AddNotification(Notification notification);
    }
}
