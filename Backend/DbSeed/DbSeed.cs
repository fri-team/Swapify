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

namespace FRITeam.Swapify.Backend.DbSeed
{
    public static class DbSeed
    {
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
                    Name = "Oleg",
                    Surname = "Dementov",
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    UserName = email,
                    NormalizedUserName = email.ToUpper(),
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    Student = await CreateStudentAsync(serviceProvider)
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
            var course1 = await courseService.FindByNameAsync("technické prostriedky PC");
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

        private static async Task<Student> CreateStudentAsync(IServiceProvider serviceProvider)
        {
            var dbService = serviceProvider.GetRequiredService<IMongoDatabase>();
            var studentCollection = dbService.GetCollection<Student>(nameof(Student));
            var studentNumberService = serviceProvider.GetRequiredService<IPersonalNumberService>();

            Student student = new Student();
            PersonalNumber sg = await studentNumberService.GetPersonalNumberAsync("558188");
            student.Timetable = sg.Timetable.Clone();
            student.PersonalNumber = sg;

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

            Dictionary<string, Course> dic = new Dictionary<string, Course>();
            foreach (var crs in courses)
            {
                dic[crs.CourseCode] = new Course()
                {
                    CourseCode = crs.CourseCode,
                    CourseName = crs.CourseName
                };
            }
            courseCollection.InsertMany(dic.Select(x => x.Value));
        }
    }
}
