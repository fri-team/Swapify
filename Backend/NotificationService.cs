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

        public async Task<IEnumerable<Notification>> GetUserNotifications(Guid userId)
        {
            return await NotificationCollection.Find(notification => notification.RecipientId == userId).ToListAsync();
        }

        public async Task AddNotification(Notification notification)
        {            
            await NotificationCollection.InsertOneAsync(notification);
        }
    }
}
