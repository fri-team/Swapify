using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.SwapifyBase.Entities.Notifications;
using MongoDB.Driver;

namespace FRITeam.Swapify.Backend
{
    public class NotificationService: INotificationService
    {        
        private readonly IMongoDatabase _database;

        private IMongoCollection<Notification> NotificationCollection  => _database.GetCollection<Notification>(nameof(Notification));

        public NotificationService(IMongoDatabase database)
        {
            _database = database;            
        }        

        public async Task UpdateNotificationReadState(Guid notificationId, bool read)
        {
            await NotificationCollection.UpdateOneAsync(
                notification => notification.Id == notificationId,
                Builders<Notification>.Update.Set(notification => notification.Read, read));
        }

        public async Task<IEnumerable<Notification>> GetStudentNotifications(Guid studentId)
        {
            return await NotificationCollection.Find(notification => notification.RecipientId == studentId).ToListAsync();
        }

        public async Task AddNotification(Notification notification)
        {            
            await NotificationCollection.InsertOneAsync(notification);
        }
    }
}
