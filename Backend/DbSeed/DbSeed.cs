using FRITeam.Swapify.Backend.CourseParser;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FRITeam.Swapify.Backend.DbSeed
{
    public static class DbSeed
    {
        public static void CreateTestingUser(IServiceProvider serviceProvider)
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
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };

                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "Heslo123");
                user.PasswordHash = hashed;
                usersCollection.InsertOne(user);
            }
        }

        public static void CreateTestingCourses(IServiceProvider serviceProvider)
        {
            var dbService = serviceProvider.GetRequiredService<IMongoDatabase>();
            var courseCollection = dbService.GetCollection<Course>(nameof(Course));

            var json = File.ReadAllText(@"..\CoursesParser\ActualCoursesJson\courses_2018_11_20.json");
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
