using FRITeam.Swapify.Backend.CourseParser;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Backend.Settings;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FRITeam.Swapify.Entities.Notifications;

namespace FRITeam.Swapify.Backend.DbSeed
{
    public static class DbSeed
    {
        private static readonly Guid OlegGuid = Guid.Parse("180ce481-85a3-4246-93b5-ba0a0229c59f");
        private static readonly Guid Oleg2Guid = Guid.Parse("60030252-5873-4fe4-b32e-9c7e0d5e3517");
        private static readonly Guid Oleg3Guid = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7");
        private static readonly Guid OlegStudentGuid = Guid.Parse("72338e48-9829-47b5-a666-766bbbecd799");
        private static readonly Guid OlegStudent2Guid = Guid.NewGuid();
        private static readonly Guid OlegStudent3Guid = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e");

        public static async Task CreateTestingUserAsync(IMongoDatabase database)
        {
            var _userCollection = database.GetCollection<User>(nameof(User));

            string email = "oleg@swapify.com";
            User oleg = _userCollection.Find(x => x.Email == email).SingleOrDefault();
            if (oleg == null)
            {
                User user = new User
                {
                    Id = OlegGuid,
                    Name = "Oleg",
                    Surname = "Dementov",
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    UserName = email,
                    NormalizedUserName = email.ToUpper(),
                    EmailConfirmed = true,
                    SecurityStamp = OlegGuid.ToString("D"),
                    Student = await CreateStudentAsync(database, OlegStudentGuid, OlegGuid)
                };

                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "Heslo123");
                user.PasswordHash = hashed;
                _userCollection.InsertOne(user);
            }

            email = "oleg2@swapify.com";
            oleg = _userCollection.Find(x => x.Email == email).SingleOrDefault();
            if (oleg == null)
            {
                User user = new User
                {
                    Id = Oleg2Guid,
                    Name = "Oleg2",
                    Surname = "Dementov",
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    UserName = email,
                    NormalizedUserName = email.ToUpper(),
                    EmailConfirmed = true,
                    SecurityStamp = Oleg2Guid.ToString("D"),
                    Student = await CreateStudentAsync(database, OlegStudent2Guid, Oleg2Guid)
                };

                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "Heslo123");
                user.PasswordHash = hashed;
                _userCollection.InsertOne(user);
            }

            email = "oleg3@swapify.com";
            oleg = _userCollection.Find(x => x.Email == email).SingleOrDefault();
            if (oleg == null)
            {
                User user = new User
                {
                    Id = Oleg3Guid,
                    Name = "Oleg3",
                    Surname = "Dementov",
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    UserName = email,
                    NormalizedUserName = email.ToUpper(),
                    EmailConfirmed = true,
                    SecurityStamp = Oleg3Guid.ToString("D"),
                    Student = await CreateStudentAsync(database, OlegStudent3Guid, Oleg3Guid)
                };

                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "Heslo123");
                user.PasswordHash = hashed;
                _userCollection.InsertOne(user);
            }
        }

        public static async Task CreateTestingExchangesAsync(IServiceProvider serviceProvider, IMongoDatabase database)
        {
            var blockExchangeCollection = database.GetCollection<BlockChangeRequest>(nameof(BlockChangeRequest));
            var usersCollection = database.GetCollection<User>("users");
            var courseCollection = database.GetCollection<Course>(nameof(Course));

            string email = "oleg@swapify.com";
            User oleg = usersCollection.Find(x => x.Email == email).SingleOrDefault();

            var course1 = await courseCollection.Find(x => x.CourseName.Equals("technické prostriedky PC")).FirstOrDefaultAsync();
            var course2 = await courseCollection.Find(x => x.CourseName.Equals("multimediálne informačné systémy")).FirstOrDefaultAsync();

            if (oleg != null)
            {
                blockExchangeCollection.InsertOne(new BlockChangeRequest
                {
                    StudentId = oleg.Student.Id,
                    Status = ExchangeStatus.WaitingForExchange,
                    DateOfCreation = DateTime.Now,
                    BlockFrom = new Block
                    {
                        CourseId = course1.Id,
                        Day = Day.Wednesday,
                        StartHour = 8
                    },
                    BlockTo = new Block
                    {
                        CourseId = course1.Id,
                        Day = Day.Thursday,
                        StartHour = 12
                    }
                });

                blockExchangeCollection.InsertOne(new BlockChangeRequest
                {
                    StudentId = oleg.Student.Id,
                    Status = ExchangeStatus.WaitingForExchange,
                    DateOfCreation = DateTime.Now,
                    BlockFrom = new Block
                    {
                        CourseId = course2.Id,
                        Day = Day.Thursday,
                        StartHour = 9
                    },
                    BlockTo = new Block
                    {
                        CourseId = course2.Id,
                        Day = Day.Friday,
                        StartHour = 15
                    }
                });
            }
        }

        public static async Task CreateTestingNotifications(IServiceProvider serviceProvider, IMongoDatabase database)
        {
            var notificationsCollection = database.GetCollection<Notification>(nameof(Notification));

            // insert testing notification only if notifications collection is empty
            if (notificationsCollection.CountDocuments(notification => true) > 0)
                return;

            var notifications = new List<Notification>
            {
                new SimpleMessageNotification()
                {
                    Id = Guid.Parse("2548a9d8-c5dc-4598-9240-c41f2a677c75"),
                    RecipientId = OlegStudentGuid,
                    Type = NotificationType.SimpleMessageNotification,
                    Message = "notifikacia 1",
                    CreatedAt = DateTime.Now,
                    Read = false
                },
                new SimpleMessageNotification()
                {
                    Id = Guid.Parse("6ed215e5-3e84-4487-b397-afe626f37a8f"),
                    RecipientId = OlegStudentGuid,
                    Type = NotificationType.SimpleMessageNotification,
                    Message = "notifikacia 2",
                    CreatedAt = DateTime.Now,
                    Read = false
                },
                new SimpleMessageNotification()
                {
                    Id = Guid.Parse("4e6cc595-5a84-4184-b06d-363d341ddbfb"),
                    RecipientId = OlegStudentGuid,
                    Type = NotificationType.SimpleMessageNotification,
                    Message = "notifikacia 3",
                    CreatedAt = DateTime.Now,
                    Read = false
                },
                new SimpleMessageNotification()
                {
                    Id = Guid.Parse("321996ff-2881-4cb8-bc55-b19f2935ae7e"),
                    RecipientId = OlegStudentGuid,
                    Type = NotificationType.SimpleMessageNotification,
                    Message = "notifikacia 4",
                    CreatedAt = DateTime.Now,
                    Read = true
                }
            };

            await notificationsCollection.InsertManyAsync(notifications);
        }

        private static async Task<Student> CreateStudentAsync(IMongoDatabase database, Guid studentId = default(Guid), Guid userId = default(Guid))
        {
            var studentCollection = database.GetCollection<Student>(nameof(Student));

            Student student = new Student
            {
                Id = (studentId == default(Guid) ? Guid.NewGuid() : studentId),
                Timetable = new Timetable(),
                PersonalNumber = null,
                UserId = userId
            };

            studentCollection.InsertOne(student);
            return student;
        }

        public static void CreateTestingCourses(IServiceProvider serviceProvider, IMongoDatabase database)
        {
            var courseCollection = database.GetCollection<Course>(nameof(Course));

            var path = serviceProvider.GetRequiredService<IOptions<PathSettings>>();
            var json = File.ReadAllText(path.Value.CoursesJsonPath);
            var courses = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CourseItem>>(json);

            Dictionary<string, Course> dic = new Dictionary<string, Course>();
            foreach (var crs in courses)
            {
                Course course = new Course()
                {
                    Id = Guid.NewGuid(),
                    CourseCode = crs.CourseCode,
                    CourseName = crs.CourseName
                };
                dic[crs.CourseCode] = course;
            }

            courseCollection.DeleteMany(x => x != null);
            courseCollection.InsertMany(dic.Select(x => x.Value));
        }
    }
}
