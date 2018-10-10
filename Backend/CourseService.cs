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

        public async Task<Guid> GetOrAddNotExistsCourseId(string courseName, ICourseService courseServ, Block courseBlock)
        {
            var course = await courseServ.FindByNameAsync(courseName);
            if (course == null)
            {
                var timetable = new Timetable();
                timetable.AddNewBlock(courseBlock);
                course = new Course() { CourseName = courseName, Timetable = timetable };
                await courseServ.AddAsync(course);
            }
            else {
                if (!course.Timetable.ContainsBlock(courseBlock))
                {
                    //if course exist but doesnt contain this block
                    //is is neccessary to add it into timetable
                    course.Timetable.AddNewBlock(courseBlock);
                    await courseServ.UpdateAsync(course);
                }
            }
            return course.Id;

        }

        public async Task UpdateAsync(Course course)
        {
            await _courseCollection.ReplaceOneAsync(x =>x.Id == course.Id,course);
        }
    }
}
