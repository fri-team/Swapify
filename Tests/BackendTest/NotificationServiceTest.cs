using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using FRITeam.Swapify.Entities.Notifications;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using Xunit;

namespace BackendTest
{
    [CollectionDefinition("Database collection")]
    public class NotificationServiceTest : IClassFixture<Mongo2GoFixture>
    {        
        private IMongoClient _mongoClient;
        private const string TestingDatabaseName = "TestingDatabaseName";

        private IList<Notification> _notifications;
        private static readonly Guid NotificationId = Guid.Parse("2548a9d8-c5dc-4598-9240-c41f2a677c75");
        private static readonly Guid TestStudentGuid = Guid.Parse("180ce481-85a3-4246-93b5-ba0a0229c59f");
        private static readonly Guid TestUserGuid = Guid.Parse("180ce481-85a3-4246-93b5-ba0a0229c59f");
        private static readonly DateTime DateTime = DateTime.Parse("2019-06-24 10:26:55");

        public NotificationServiceTest(Mongo2GoFixture mongo2GoFixture)
        {
            _mongoClient = mongo2GoFixture.MongoClient;
            InitTestStudentNotifications();
        }        

        [Fact]
        public async Task GetStudentNotifications()
        {
            IMongoDatabase database = await InitializeDatabaseWithNotificationsUserStudent(_mongoClient, TestingDatabaseName);
            var notificationService = new NotificationService(database);

            var notifications = await notificationService.GetStudentNotifications(TestStudentGuid);

            notifications.Should().BeEquivalentTo(_notifications);
        }

        [Fact]
        public async Task UpdateNotificationReadState()
        {
            IMongoDatabase database = await InitializeDatabaseWithNotifications(_mongoClient, TestingDatabaseName);
            var notificationService = new NotificationService(database);

            await notificationService.UpdateNotificationReadState(NotificationId, true);

            var notifications = (await notificationService.GetStudentNotifications(TestStudentGuid)).ToList();

            // modify collection to expected state
            _notifications.First(notification => notification.Id == NotificationId).Read = true; ;
                        
            notifications.Should().BeEquivalentTo(_notifications);
        }

        [Fact]
        public async Task AddNotification()
        {
            IMongoDatabase database = await InitializeDatabaseWithNotifications(_mongoClient, TestingDatabaseName);
            var notificationService = new NotificationService(database);
            
            var newNotification = new SimpleMessageNotification()
            {
                Id = Guid.NewGuid(),
                RecipientId = TestStudentGuid,
                Type = NotificationType.SimpleMessageNotification,
                Message = "nova notifikacia",
                CreatedAt = DateTime,
                Read = true
            };
            
            await notificationService.AddNotification(newNotification);

            var notifications = (await notificationService.GetStudentNotifications(TestStudentGuid)).ToList();

            notifications.Should().BeEquivalentTo(
                _notifications.Concat(new List<Notification>() { newNotification }));
        }

        private async Task<IMongoDatabase> InitializeDatabaseWithNotifications(IMongoClient mongoClient, string testingDatabaseName)
        {
            await _mongoClient.DropDatabaseAsync(testingDatabaseName);

            IMongoDatabase testingDatabase = _mongoClient.GetDatabase(testingDatabaseName);            
            await CreateTestingNotificationsAsync(testingDatabase);

            return testingDatabase;
        }

        private async Task<IMongoDatabase> InitializeDatabaseWithNotificationsUserStudent(IMongoClient mongoClient, string testingDatabaseName)
        {
            await _mongoClient.DropDatabaseAsync(testingDatabaseName);

            IMongoDatabase testingDatabase = _mongoClient.GetDatabase(testingDatabaseName);

            await CreateTestingUserAsync(testingDatabase);
            await CreateTestingNotificationsAsync(testingDatabase);

            return testingDatabase;
        }

        private void InitTestStudentNotifications()
        {
            _notifications = new List<Notification>
            {
                new SimpleMessageNotification()
                {
                    Id = NotificationId,
                    RecipientId = TestStudentGuid,
                    Type = NotificationType.SimpleMessageNotification,
                    Message = "notifikacia 1",
                    CreatedAt = DateTime,
                    Read = false
                },
                new SimpleMessageNotification()
                {
                    Id = Guid.NewGuid(),
                    RecipientId = TestStudentGuid,
                    Type = NotificationType.SimpleMessageNotification,
                    Message = "notifikacia 2",
                    CreatedAt = DateTime,
                    Read = false
                },
                new SimpleMessageNotification()
                {
                    Id = Guid.NewGuid(),
                    RecipientId = TestStudentGuid,
                    Type = NotificationType.SimpleMessageNotification,
                    Message = "notifikacia 3",
                    CreatedAt = DateTime,
                    Read = false
                },
                new SimpleMessageNotification()
                {
                    Id = Guid.NewGuid(),
                    RecipientId = TestStudentGuid,
                    Type = NotificationType.SimpleMessageNotification,
                    Message = "notifikacia 4",
                    CreatedAt = DateTime,
                    Read = true
                }
            };
        }        

        private async Task CreateTestingNotificationsAsync(IMongoDatabase database)
        {
            IMongoCollection<Notification> notificationCollection = database.GetCollection<Notification>(nameof(Notification));
            await notificationCollection.InsertManyAsync(_notifications);
        }

        private async Task CreateTestingUserAsync(IMongoDatabase database)
        {
            IMongoCollection<User> usersCollection = database.GetCollection<User>("users");

            string email = "oleg@swapify.com";
            User oleg = usersCollection.Find(x => x.Email == email).SingleOrDefault();
            if (oleg == null)
            {
                User user = new User
                {
                    Id = TestUserGuid,
                    Name = "Oleg",
                    Surname = "Dementov",
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    UserName = email,
                    NormalizedUserName = email.ToUpper(),
                    EmailConfirmed = true,
                    SecurityStamp = TestUserGuid.ToString("D"),
                    Student = await CreateStudentAsync(database, TestStudentGuid)
                };

                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "Heslo123");
                user.PasswordHash = hashed;
                usersCollection.InsertOne(user);
            }
        }

        private async Task<Student> CreateStudentAsync(IMongoDatabase database, Guid studentId = default(Guid))
        {            
            var studentCollection = database.GetCollection<Student>(nameof(Student));

            if (studentId == default(Guid))
            {
                studentId = Guid.NewGuid();
            }

            Student student = new Student
            {
                Id = studentId,
                Timetable = null,
                PersonalNumber = null
            };

            await studentCollection.InsertOneAsync(student);
            return student;
        }                
    }
}
