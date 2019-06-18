using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FRITeam.Swapify.Backend
{
    public class NotificationService: INotificationService
    {
        private readonly ILogger<StudyGroupService> _logger;
        private readonly IMongoDatabase _database;

        private IMongoCollection<Notification> NotificationCollection  => _database.GetCollection<Notification>(nameof(Notification));

        public NotificationService(IMongoDatabase database, ILogger<StudyGroupService> logger)
        {
            _database = database;
            _logger = logger;            
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
