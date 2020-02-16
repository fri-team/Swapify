using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Notifications;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FRITeam.Swapify.Backend
{
    public class NotificationService: INotificationService
    {
        private readonly IMongoCollection<Notification> _notificationCollection;

        public NotificationService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _notificationCollection = database.GetCollection<Notification>(nameof(Notification));
        }

        public async Task UpdateNotificationReadState(Guid notificationId, bool read)
        {
            await _notificationCollection.UpdateOneAsync(
                notification => notification.Id == notificationId,
                Builders<Notification>.Update.Set(notification => notification.Read, read));
        }

        public async Task<IEnumerable<Notification>> GetStudentNotifications(Guid studentId)
        {
            return await _notificationCollection.Find(notification => notification.RecipientId == studentId).ToListAsync();
        }

        public async Task AddNotification(Notification notification)
        {
            await _notificationCollection.InsertOneAsync(notification);
        }
    }
}
