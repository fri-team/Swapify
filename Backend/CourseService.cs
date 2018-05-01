using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.Backend.Converter;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using MongoDB.Driver;

namespace FRITeam.Swapify.Backend
{
    public class CourseService : ICourseService
    {
        private readonly IMongoDatabase _database;
        private IMongoCollection<Course> _courseCollection => _database.GetCollection<Course>(nameof(Course));

        public CourseService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(Course entityToAdd)
        {
            entityToAdd.Id = Guid.NewGuid();
            await _courseCollection.InsertOneAsync(entityToAdd);
        }

        public async Task<Course> FindByIdAsync(Guid guid)
        {
            return await _courseCollection.Find(x => x.Id.Equals(guid)).FirstOrDefaultAsync();
        }

        public async Task<Course> FindByNameAsync(string name)
        {
            return await _courseCollection.Find(x => x.CourseName.Equals(name)).FirstOrDefaultAsync();
        }

        public async Task<Guid> GetOrAddNotExistsCourseId(string courseName, ICourseService courseServ, Block bl)
        {
            var course = await courseServ.FindByNameAsync(courseName);
            if (course == null)
            {
                var timetable = new Timetable();
                timetable.Blocks.Add(bl);
                course = new Course() { CourseName = courseName, Timetable = timetable };
                await courseServ.AddAsync(course);
            }
            else {
                if (!course.Timetable.ContainsBlock(bl))
                {
                    //if course exist but doesnt contain this block
                    //is is neccessary to add it into timetable
                    course.Timetable.Blocks.Add(bl);
                    await courseServ.Update(course);
                }
            }
            return course.Id;

        }

        public async Task Update(Course course)
        {
            await _courseCollection.ReplaceOneAsync(x =>x.Id == course.Id,course);
        }
    }
}
