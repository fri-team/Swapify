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
using System.Text;

namespace FRITeam.Swapify.Backend.DbSeed
{
    public static class DbSeed
    {
        private static readonly Guid OlegGuid = Guid.NewGuid();
        private static readonly Guid Oleg2Guid = Guid.NewGuid();
        private static readonly Guid Oleg3Guid = Guid.NewGuid();
        private static readonly Guid NikitaGuid = Guid.NewGuid();
        private static readonly Guid OlegStudentGuid = Guid.NewGuid();
        private static readonly Guid OlegStudent2Guid = Guid.NewGuid();
        private static readonly Guid OlegStudent3Guid = Guid.NewGuid();
        private static readonly Guid NikitaStudentGuid = Guid.NewGuid();

        public static async Task CreateTestingUserAsync(IServiceProvider serviceProvider)
        {
            var dbService = serviceProvider.GetRequiredService<IMongoDatabase>();
            var usersCollection = dbService.GetCollection<User>("users");            

            string email = "oleg@swapify.com";
            User oleg = usersCollection.Find(x => x.Email == email).SingleOrDefault();
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
                    Student = await CreateStudentAsync(serviceProvider, OlegStudentGuid, OlegGuid)
                };
                
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "Heslo123");
                user.PasswordHash = hashed;
                usersCollection.InsertOne(user);
            }

            email = "oleg2@swapify.com";
            oleg = usersCollection.Find(x => x.Email == email).SingleOrDefault();
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
                    Student = await CreateStudentAsync(serviceProvider, OlegStudent2Guid, Oleg2Guid)
                };

                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "Heslo123");
                user.PasswordHash = hashed;
                usersCollection.InsertOne(user);
            }

            email = "oleg3@swapify.com";
            oleg = usersCollection.Find(x => x.Email == email).SingleOrDefault();
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
                    Student = await CreateStudentAsync(serviceProvider, OlegStudent3Guid, Oleg3Guid)
                };

                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "Heslo123");
                user.PasswordHash = hashed;
                usersCollection.InsertOne(user);
            }

            email = "nikita@swapify.com";
            User nikita = usersCollection.Find(x => x.Email == email).SingleOrDefault();
            if (nikita == null)
            {
                User user = new User
                {
                    Id = NikitaGuid,
                    Name = "Nikita",
                    Surname = "Dementov",
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    UserName = email,
                    NormalizedUserName = email.ToUpper(),
                    EmailConfirmed = false,
                    SecurityStamp = NikitaGuid.ToString("D"),
                    Student = await CreateStudentAsync(serviceProvider, NikitaStudentGuid, NikitaGuid)
                };

                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "Heslo123");
                user.PasswordHash = hashed;
                usersCollection.InsertOne(user);
            }
        }

        public static async Task CreateTestingExchangesAsync(IServiceProvider serviceProvider)
        {
            var dbService = serviceProvider.GetRequiredService<IMongoDatabase>();
            var blockExchangeCollection = dbService.GetCollection<BlockChangeRequest>(nameof(BlockChangeRequest));
            var usersCollection = dbService.GetCollection<User>("users");

            string email = "oleg@swapify.com";
            User oleg = usersCollection.Find(x => x.Email == email).SingleOrDefault();

            var courseService = serviceProvider.GetRequiredService<ICourseService>();
            var course2 = await courseService.FindByNameAsync("multimediálne informačné systémy");

            if (oleg != null)
            {
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

        public static async Task CreateTestingNotifications(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetRequiredService<IMongoDatabase>();
            var notificationsCollection = db.GetCollection<Notification>(nameof(Notification));

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

        private static async Task<Student> CreateStudentAsync(IServiceProvider serviceProvider, Guid studentId = default(Guid), Guid userId = default(Guid))
        {
            var dbService = serviceProvider.GetRequiredService<IMongoDatabase>();
            var studentCollection = dbService.GetCollection<Student>(nameof(Student));

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

        public static void CreateTestingCourses(IServiceProvider serviceProvider)
        {
            var dbService = serviceProvider.GetRequiredService<IMongoDatabase>();
            var courseCollection = dbService.GetCollection<Course>(nameof(Course));
            
            var path = serviceProvider.GetRequiredService<IOptions<PathSettings>>();
            var json = File.ReadAllText(path.Value.CoursesJsonPath);
            var courses = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CourseItem>>(json);

            foreach (var crs in courses)
            {
                Course course = new Course()
                {
                    Id = Guid.NewGuid(),
                    CourseCode = crs.CourseCode,
                    CourseName = crs.CourseName,
                    LastUpdateOfTimetable = null,
                    YearOfStudy = crs.YearOfStudy,
                    StudyType = crs.StudyType
                };

                Course c = courseCollection.Find(x => x.CourseCode == crs.CourseCode && x.YearOfStudy == crs.YearOfStudy && x.StudyType == crs.StudyType).SingleOrDefault();

                if (c == null)
                {
                    courseCollection.InsertOne(course);
                }
            }
        }

        public static void ImportTestDb(Mongo2Go.MongoDbRunner paRunner, String paPath)
        {
            paRunner.Import("Swapify", "BlockChangeRequest", paPath + "\\BlockChangeRequest.json", true);
            paRunner.Import("Swapify", "Course", paPath + "\\Course.json", true);
            paRunner.Import("Swapify", "Notification", paPath + "\\Notification.json", true);
            paRunner.Import("Swapify", "Student", paPath + "\\Student.json", true);
            paRunner.Import("Swapify", "users", paPath + "\\users.json", true);
        }
    }
}
