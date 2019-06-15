using FluentAssertions;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.APIWrapper.Enums;
using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Backend.Converter;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions.Common;
using FRITeam.Swapify.Backend.Interfaces;
using Xunit;

namespace BackendTest
{
    [Collection("Database collection")]
    public class ConverterTest : IClassFixture<Mongo2GoFixture>
    {
        private readonly Mongo2GoFixture _mongoFixture;
        private readonly IMongoDatabase _database;
        private readonly Mock<ILogger<StudyGroupService>> _loggerMock;
        private readonly Mock<ILogger<CourseService>> _loggerMockCourse;

        public ConverterTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
            _loggerMock = new Mock<ILogger<StudyGroupService>>();
            _loggerMockCourse = new Mock<ILogger<CourseService>>();
            _database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
        }

        [Fact]
        public async Task ConvertTest_ValidStudyGroup()
        {
            var schoolScheduleProxy = new SchoolScheduleProxy();
            var serviceCourse = new CourseService(_loggerMockCourse.Object, _database, schoolScheduleProxy);
            var service = new StudyGroupService(_loggerMock.Object, _database, schoolScheduleProxy, serviceCourse);
            StudyGroup grp = await service.GetStudyGroupAsync("5ZZS13");
            grp.Should().NotBeNull();
        }

        [Fact]
        public async Task ConvertTest_NotValidStudyGroup()
        {
            var schoolScheduleProxy = new SchoolScheduleProxy();
            var serviceCourse = new CourseService(_loggerMockCourse.Object, _database, schoolScheduleProxy);
            var serviceMock = new Mock<StudyGroupService>(_loggerMock.Object, _database, schoolScheduleProxy, serviceCourse);
            serviceMock.Setup(x => x.GetStudyGroupAsync(It.IsAny<string>()))
                       .Returns(Task.FromResult<StudyGroup>(null));

            var result = await serviceMock.Object.GetStudyGroupAsync("5ZZS99");

            Assert.Null(result);
        }

        [Fact]
        public async void ConvertTest_ConvertAndMergeSameConsecutiveBlocks()
        {

            // define schedule hour content for 4 blocks
            var block1Hour1 =
                new ScheduleHourContent(0, 1, false,
                    LessonType.Laboratory, "teacher1",
                    "room200", "sub1", "subject1", SubjectType.None);

            var block1Hour2 =
                new ScheduleHourContent(0, 2, false,
                    LessonType.Laboratory, "teacher1",
                    "room200", "sub1", "subject1", SubjectType.None);

            // block with different course ant time
            var block2Hour1 =
                new ScheduleHourContent(0, 3, false,
                    LessonType.Laboratory, "teacher1",
                    "room200", "sub2", "subject2", SubjectType.None);

            var block2Hour2 =
                new ScheduleHourContent(0, 4, false,
                    LessonType.Laboratory, "teacher1",
                    "room200", "sub2", "subject2", SubjectType.None);

            // same time as block 2
            var block3Hour1 =
                new ScheduleHourContent(0, 3, false,
                    LessonType.Laboratory, "teacher2",
                    "room201", "sub2", "subject2", SubjectType.None);

            var block3Hour2 =
                new ScheduleHourContent(0, 4, false,
                    LessonType.Laboratory, "teacher2",
                    "room201", "sub2", "subject2", SubjectType.None);

            var block4Hour1 =
                new ScheduleHourContent(4, 5, false,
                    LessonType.Laboratory, "teacher2",
                    "room201", "sub2", "subject2", SubjectType.None);

            var block4Hour2 =
                new ScheduleHourContent(4, 6, false,
                    LessonType.Laboratory, "teacher2",
                    "room201", "sub2", "subject2", SubjectType.None);


            var scheduleHourContents = new List<ScheduleHourContent>()
            {
                block3Hour1, block1Hour1, block4Hour2, block2Hour2, block1Hour2, block3Hour2, block4Hour1, block2Hour1
            };

            // create course service mock
            var guid = Guid.NewGuid();
            var courseServiceMock = new Mock<ICourseService>();
            courseServiceMock.Setup(m => m.GetOrAddNotExistsCourseId(It.IsAny<string>(), It.IsAny<Block>())).ReturnsAsync(guid);

            // execute
            var timetable = await ConverterApiToDomain.ConvertAndMergeSameConsecutiveBlocks(scheduleHourContents, courseServiceMock.Object, false);

            // define result blocks
            var expectedResult = new List<Block>()
            {
                new Block()
                {
                    Day = Day.Monday,
                    StartHour = 7,
                    Duration = 2,
                    Room = "room200",
                    Teacher = "teacher1",
                    BlockType = BlockType.Laboratory,
                    CourseId = guid
                },
                new Block()
                {
                    Day = Day.Monday,
                    StartHour = 9,
                    Duration = 2,
                    Room = "room200",
                    Teacher = "teacher1",
                    BlockType = BlockType.Laboratory,
                    CourseId = guid
                },
                new Block()
                {
                    Day = Day.Monday,
                    StartHour = 9,
                    Duration = 2,
                    Room = "room201",
                    Teacher = "teacher2",
                    BlockType = BlockType.Laboratory,
                    CourseId = guid                   
                },
                new Block()
                {
                    Day = Day.Friday,
                    StartHour = 11,
                    Duration = 2,
                    Room = "room201",
                    Teacher = "teacher2",
                    BlockType = BlockType.Laboratory,
                    CourseId = guid
                },                
            };
            
            timetable.AllBlocks.Should().BeEquivalentTo(expectedResult);
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
