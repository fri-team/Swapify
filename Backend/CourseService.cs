using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Task<List<Course>> FindByStartName(string courseStartsWith)
        {
            return _courseCollection.Find(x => x.CourseName.StartsWith(courseStartsWith)).ToListAsync();
        }

        /// <summary>
        /// If course with "courseName" exists function return ID, if course doesnt exist fuction
        /// save this course and return id of saved course.
        /// </summary>
        public async Task<Guid> GetOrAddNotExistsCourseId(string courseName, Block courseBlock)
        {
            var course = await this.FindByNameAsync(courseName);
            if (course == null)
            {
                var timetable = new Timetable();
                timetable.AddNewBlock(courseBlock);
                course = new Course() { CourseName = courseName, Timetable = timetable };
                await this.AddAsync(course);
            }
            else
            {
                if (course.Timetable == null)
                {
                    course.Timetable = new Timetable();
                }
                if (!course.Timetable.ContainsBlock(courseBlock))
                {
                    //if course exists but doesnt contain this block
                    //is it neccessary to add it into timetable
                    course.Timetable.AddNewBlock(courseBlock);
                    await this.UpdateAsync(course);
                }
            }
            return course.Id;

        }

        public async Task UpdateAsync(Course course)
        {
            await _courseCollection.ReplaceOneAsync(x => x.Id == course.Id, course);
        }
    }
}
