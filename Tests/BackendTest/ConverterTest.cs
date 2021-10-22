using FluentAssertions;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Backend.Converter;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace BackendTest
{
    [Collection("Database collection")]
    public class ConverterTest : IClassFixture<Mongo2GoFixture>
    {
        private readonly Mongo2GoFixture _mongoFixture;
        private readonly IMongoDatabase _database;
        private readonly Mock<ILogger<SchoolScheduleProxy>> _loggerMockSchedule;
        private readonly Mock<ILogger<CourseService>> _loggerMockCourse;

        public ConverterTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
            _loggerMockSchedule = new Mock<ILogger<SchoolScheduleProxy>>();
            _loggerMockCourse = new Mock<ILogger<CourseService>>();
            _database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
        }
        
        [Fact]
        public void ConvertTest_MergeSameBlocksWithDifferentTeacher()
        {
            
            var blocks = new List<Block>()
            {
                new Block()
                {
                    Day = Day.Monday,
                    StartHour = 7,
                    Duration = 2,
                    Room = "room200",
                    Teacher = "teacher1",
                    BlockType = BlockType.Laboratory,
                    CourseId = Guid.Empty
                },
                new Block()
                {
                    Day = Day.Monday,
                    StartHour = 8,
                    Duration = 2,
                    Room = "room200",
                    Teacher = "teacher1",
                    BlockType = BlockType.Laboratory,
                    CourseId = Guid.Empty
                },
                new Block()
                {
                    Day = Day.Tuesday,
                    StartHour = 7,
                    Duration = 2,
                    Room = "room200",
                    Teacher = "teacher1",
                    BlockType = BlockType.Laboratory,
                    CourseId = Guid.Empty
                },
                new Block()
                {
                    Day = Day.Tuesday,
                    StartHour = 7,
                    Duration = 2,
                    Room = "room200",
                    Teacher = "teacher2",
                    BlockType = BlockType.Laboratory,
                    CourseId = Guid.Empty
                },
                new Block()
                {
                    Day = Day.Wednesday,
                    StartHour = 9,
                    Duration = 2,
                    Room = "room200",
                    Teacher = "teacher1",
                    BlockType = BlockType.Laboratory,
                    CourseId = Guid.Empty
                },
                new Block()
                {
                    Day = Day.Wednesday,
                    StartHour = 9,
                    Duration = 2,
                    Room = "room201",
                    Teacher = "teacher1",
                    BlockType = BlockType.Laboratory,
                    CourseId = Guid.Empty
                }
            };

            var timetable = ConverterApiToDomain.MergeSameBlocksWithDifferentTeacher(blocks);

            var mergedBlocks =  new List<Block>()
            {
                new Block()
                {
                    Day = Day.Monday,
                    StartHour = 7,
                    Duration = 2,
                    Room = "room200",
                    Teacher = "teacher1",
                    BlockType = BlockType.Laboratory,
                    CourseId = Guid.Empty
                },                
                new Block()
                {
                    Day = Day.Monday,
                    StartHour = 8,
                    Duration = 2,
                    Room = "room200",
                    Teacher = "teacher1",
                    BlockType = BlockType.Laboratory,
                    CourseId = Guid.Empty
                },
                new Block()
                {
                    Day = Day.Tuesday,
                    StartHour = 7,
                    Duration = 2,
                    Room = "room200",
                    Teacher = "teacher1,teacher2",
                    BlockType = BlockType.Laboratory,
                    CourseId = Guid.Empty
                },                
                new Block()
                {
                    Day = Day.Wednesday,
                    StartHour = 9,
                    Duration = 2,
                    Room = "room200",
                    Teacher = "teacher1",
                    BlockType = BlockType.Laboratory,
                    CourseId = Guid.Empty
                },
                new Block()
                {
                    Day = Day.Wednesday,
                    StartHour = 9,
                    Duration = 2,
                    Room = "room201",
                    Teacher = "teacher1",
                    BlockType = BlockType.Laboratory,
                    CourseId = Guid.Empty
                }
            };
            timetable.AllBlocks.Should().BeEquivalentTo(mergedBlocks);            
        }
    }
}
