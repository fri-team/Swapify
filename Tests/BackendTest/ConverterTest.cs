using FluentAssertions;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.APIWrapper.Enums;
using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Backend.Converter;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BackendTest
{
    [Collection("Database collection")]
    public class ConverterTest : IClassFixture<Mongo2GoFixture>
    {

        private readonly Mongo2GoFixture _mongoFixture;

        public ConverterTest(Mongo2GoFixture mongoFixture)
        {
            _mongoFixture = mongoFixture;
        }

        [Fact]
        public async void ConvertTest_ValidGroupWithSubject()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            var service = new StudyGroupService(database);
            var serviceCourse = new CourseService(database);
            

            StudyGroup grp = await service.GetStudyGroupAsync("5ZZS13", serviceCourse, new SchoolScheduleProxy());
            StudyGroup grp1 = await service.GetStudyGroupAsync("5ZZS14", serviceCourse, new SchoolScheduleProxy());

            var crs = await serviceCourse.FindByNameAsync("teória informácie");
#pragma warning disable S1067 // Expressions should not be too complex
            crs.Timetable.Blocks.Where(x => (x.Day == Day.Monday) && (x.Duration == 2) &&
                                            (x.Room == "RA301") && (x.StartHour == 11) &&
                                            (x.BlockType == BlockType.Laboratory)).Should().NotBeNull();
            
            crs.Timetable.Blocks.Where(x => (x.Day == Day.Monday) && (x.Duration == 2) &&
                                            (x.Room == "RA301") && (x.StartHour == 13) &&
                                            (x.BlockType == BlockType.Laboratory)).Should().NotBeNull();

            crs.Timetable.Blocks.Where(x => (x.Day == Day.Thursday) && (x.Duration == 2) &&
                                            (x.Room == "RC009") && (x.StartHour == 12) &&
                                            (x.BlockType == BlockType.Lecture)).Should().NotBeNull();

            
            crs = await serviceCourse.FindByNameAsync("architektúry informačných systémov");
            crs.Timetable.Blocks.Where(x => (x.Day == Day.Monday) && (x.Duration == 2) &&
                                            (x.Room == "RB003") && (x.StartHour == 17) &&
                                            (x.BlockType == BlockType.Laboratory)).Should().NotBeNull();

            crs.Timetable.Blocks.Where(x => (x.Day == Day.Wednesday) && (x.Duration == 2) &&
                                            (x.Room == "RB003") && (x.StartHour == 16) &&
                                            (x.BlockType == BlockType.Laboratory)).Should().NotBeNull();

            crs.Timetable.Blocks.Where(x => (x.Day == Day.Thursday) && (x.Duration == 2) &&
                                            (x.Room == "RC009") && (x.StartHour == 12) &&
                                            (x.BlockType == BlockType.Lecture)).Should().NotBeNull();


            crs = await serviceCourse.FindByNameAsync("databázy a získavanie znalostí");
            crs.Timetable.Blocks.Where(x => (x.Day == Day.Monday) && (x.Duration == 2) &&
                                            (x.Room == "RA013") && (x.StartHour == 13) &&
                                            (x.BlockType == BlockType.Laboratory)).Should().NotBeNull();

            crs.Timetable.Blocks.Where(x => (x.Day == Day.Friday) && (x.Duration == 2) &&
                                            (x.Room == "RB052") && (x.StartHour == 12) &&
                                            (x.BlockType == BlockType.Laboratory)).Should().NotBeNull();

            crs.Timetable.Blocks.Where(x => (x.Day == Day.Thursday) && (x.Duration == 2) &&
                                            (x.Room == "RC009") && (x.StartHour == 7) &&
                                            (x.BlockType == BlockType.Lecture)).Should().NotBeNull();
#pragma warning restore S1067 // Expressions should not be too complex
        }


        [Fact]
        public async void ConvertTest_ValidStudyGroup()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            var service = new StudyGroupService(database);
            var serviceCourse = new CourseService(database);

            StudyGroup grp = await service.GetStudyGroupAsync("5ZZS13", serviceCourse, new FakeProxy());
            grp.Should().NotBeNull();
        }

        [Fact]
        public async void ConvertTest_NotValidStudyGroup()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            var service = new StudyGroupService(database);
            var serviceCourse = new CourseService(database);

            Assert.Throws<AggregateException>(() => service.GetStudyGroupAsync("5ZZS99", serviceCourse, new SchoolScheduleProxy()).Wait());
        }


        [Fact]
        public async void ConvertTest_ValidStudyGroup1()
        {
            IMongoDatabase database = _mongoFixture.MongoClient.GetDatabase("StudentsDB");
            var serviceCourse = new CourseService(database);

            ScheduleDayContent day = new ScheduleDayContent();
            var grps = new List<string>();
            grps.Add("5ZZS12");
            grps.Add("5ZZS12");
            grps.Add("5ZZS13");
            grps.Add("5ZZS14");

            ScheduleHourContent slot1 = new ScheduleHourContent(1, false, LessonType.Excercise, "teacher1", "room200", "AA", "sbj", SubjectType.Compulsory, grps);
            ScheduleHourContent slot2 = new ScheduleHourContent(2, false, LessonType.Excercise, "teacher1", "room200", "AA", "sbj", SubjectType.Compulsory, grps);
            ScheduleHourContent slot3 = null;
            ScheduleHourContent slot4 = null;
            ScheduleHourContent slot5 = new ScheduleHourContent(5, false, LessonType.Excercise, "teacher5", "room2005", "AA5", "sbj5", SubjectType.Optional, grps);
            ScheduleHourContent slot6 = null;
            ScheduleHourContent slot7 = new ScheduleHourContent(7, false, LessonType.Excercise, "teacher7", "room2007", "AA7", "sbj7", SubjectType.Compulsory, grps);
            ScheduleHourContent slot8 = new ScheduleHourContent(8, false, LessonType.Laboratory, "teacher8", "room2008", "AA8", "sbj8", SubjectType.Compulsory, grps);
            ScheduleHourContent slot9 = null;
            ScheduleHourContent slot10 = new ScheduleHourContent(10, false, LessonType.Excercise, "teacher10", "room20010", "AA10", "sbj10", SubjectType.Compulsory, grps);
            ScheduleHourContent slot11 = new ScheduleHourContent(11, false, LessonType.Excercise, "teacher10", "room20010", "AA10", "sbj10", SubjectType.Compulsory, grps);
            ScheduleHourContent slot12 = null;
            ScheduleHourContent slot13 = new ScheduleHourContent(13, false, LessonType.Excercise, "teacher13", "room20013", "AA13", "sbj13", SubjectType.Elective, grps);

            day.BlocksInDay.Add(slot1);
            day.BlocksInDay.Add(slot2);
            day.BlocksInDay.Add(slot3);
            day.BlocksInDay.Add(slot4);
            day.BlocksInDay.Add(slot5);
            day.BlocksInDay.Add(slot6);
            day.BlocksInDay.Add(slot7);
            day.BlocksInDay.Add(slot8);
            day.BlocksInDay.Add(slot9);
            day.BlocksInDay.Add(slot10);
            day.BlocksInDay.Add(slot11);
            day.BlocksInDay.Add(slot12);
            day.BlocksInDay.Add(slot13);

            ScheduleWeekContent week = new ScheduleWeekContent();
            week.DaysInWeek.Add(day);
            var timetable = await ConverterApiToDomain.ConvertTimetableForGroupAsync(week, serviceCourse, new FakeProxy());

            timetable.Blocks.Count.Should().Be(6);
            var blok = timetable.Blocks.FirstOrDefault(x => x.StartHour == 7);
            blok.Room.Should().Be("room200");
            blok.StartHour.Should().Be(7);
            blok.Teacher.Should().Be("teacher1");
            blok.Duration.Should().Be(2);

            blok = timetable.Blocks.FirstOrDefault(x => x.StartHour == 11);
            blok.Room.Should().Be("room2005");
            blok.Day.Should().Be(Day.Monday);
            blok.Teacher.Should().Be("teacher5");
            blok.Duration.Should().Be(1);

            blok = timetable.Blocks.FirstOrDefault(x => x.StartHour == 13);
            blok.Room.Should().Be("room2007");
            blok.Day.Should().Be(Day.Monday);
            blok.Teacher.Should().Be("teacher7");
            blok.Duration.Should().Be(1);

            blok = timetable.Blocks.FirstOrDefault(x => x.StartHour == 16);
            blok.Room.Should().Be("room20010");
            blok.Day.Should().Be(Day.Monday);
            blok.Teacher.Should().Be("teacher10");
            blok.Duration.Should().Be(2);

            blok = timetable.Blocks.FirstOrDefault(x => x.StartHour == 19);
            blok.Room.Should().Be("room20013");
            blok.Day.Should().Be(Day.Monday);
            blok.Teacher.Should().Be("teacher13");
            blok.Duration.Should().Be(1);
        }
    }

    public class FakeProxy : ISchoolScheduleProxy
    {
        public ScheduleWeekContent GetByRoomNumber(string roomNumber)
        {
            return GetSchedule();
        }

        public ScheduleWeekContent GetByStudyGroup(string studyGroupNumber)
        {
            return GetSchedule();
        }

        public ScheduleWeekContent GetBySubjectCode(string subjectCode)
        {
            return GetScheduleForSubject();
        }

        public ScheduleWeekContent GetByTeacherName(string teacherNumber)
        {
            return GetSchedule();
        }
        private ScheduleWeekContent GetScheduleForSubject()
        {
            ScheduleDayContent day = new ScheduleDayContent();
            var grps = new List<string>();
            grps.Add("5ZZS12");
            grps.Add("5ZZS12");
            grps.Add("5ZZS13");
            grps.Add("5ZZS14");

            var grps2 = new List<string>();
            grps.Add("5ZZS18");
            grps.Add("5ZZS17");

            ScheduleHourContent slot1 = new ScheduleHourContent(1, false, LessonType.Excercise, "teacher1", "room200", "AA", "sbj", SubjectType.Compulsory, grps);
            ScheduleHourContent slot2 = new ScheduleHourContent(2, false, LessonType.Excercise, "teacher1", "room200", "AA", "sbj", SubjectType.Compulsory, grps);
            ScheduleHourContent slot3 = null;
            ScheduleHourContent slot4 = null;
            ScheduleHourContent slot5 = null;
            ScheduleHourContent slot6 = null;
            ScheduleHourContent slot7 = new ScheduleHourContent(7, false, LessonType.Excercise, "teacher1", "room200", "AA", "sbj", SubjectType.Compulsory, grps);
            ScheduleHourContent slot8 = new ScheduleHourContent(8, false, LessonType.Excercise, "teacher1", "room200", "AA", "sbj", SubjectType.Compulsory, grps);
            ScheduleHourContent slot9 = null;
            ScheduleHourContent slot10 = new ScheduleHourContent(10, false, LessonType.Excercise, "teacher1", "room200", "AA", "sbj", SubjectType.Compulsory, grps2);
            ScheduleHourContent slot11 = new ScheduleHourContent(11, false, LessonType.Excercise, "teacher1", "room200", "AA", "sbj", SubjectType.Compulsory, grps2);
            ScheduleHourContent slot12 = new ScheduleHourContent(12, false, LessonType.Excercise, "teacher1", "room200", "AA", "sbj", SubjectType.Compulsory, grps);
            ScheduleHourContent slot13 = new ScheduleHourContent(13, false, LessonType.Excercise, "teacher1", "room200", "AA", "sbj", SubjectType.Compulsory, grps);

            day.BlocksInDay.Add(slot1);
            day.BlocksInDay.Add(slot2);
            day.BlocksInDay.Add(slot3);
            day.BlocksInDay.Add(slot4);
            day.BlocksInDay.Add(slot5);
            day.BlocksInDay.Add(slot6);
            day.BlocksInDay.Add(slot7);
            day.BlocksInDay.Add(slot8);
            day.BlocksInDay.Add(slot9);
            day.BlocksInDay.Add(slot10);
            day.BlocksInDay.Add(slot11);
            day.BlocksInDay.Add(slot12);
            day.BlocksInDay.Add(slot13);

            ScheduleWeekContent week = new ScheduleWeekContent();
            week.DaysInWeek.Add(day);
            return week;
        }

        private ScheduleWeekContent GetSchedule()
        {
            ScheduleDayContent day = new ScheduleDayContent();
            var grps = new List<string>();
            grps.Add("5ZZS12");
            grps.Add("5ZZS12");
            grps.Add("5ZZS13");
            grps.Add("5ZZS14");

            ScheduleHourContent slot1 = new ScheduleHourContent(1, false, LessonType.Excercise, "teacher1", "room200", "AA", "sbj", SubjectType.Compulsory, grps);
            ScheduleHourContent slot2 = new ScheduleHourContent(2, false, LessonType.Excercise, "teacher1", "room200", "AA", "sbj", SubjectType.Compulsory, grps);
            ScheduleHourContent slot3 = null;
            ScheduleHourContent slot4 = null;
            ScheduleHourContent slot5 = new ScheduleHourContent(5, false, LessonType.Excercise, "teacher5", "room2005", "AA5", "sbj5", SubjectType.Optional, grps);
            ScheduleHourContent slot6 = null;
            ScheduleHourContent slot7 = new ScheduleHourContent(7, false, LessonType.Excercise, "teacher7", "room2007", "AA7", "sbj7", SubjectType.Compulsory, grps);
            ScheduleHourContent slot8 = new ScheduleHourContent(8, false, LessonType.Laboratory, "teacher8", "room2008", "AA8", "sbj8", SubjectType.Compulsory, grps);
            ScheduleHourContent slot9 = null;
            ScheduleHourContent slot10 = new ScheduleHourContent(10, false, LessonType.Excercise, "teacher10", "room20010", "AA10", "sbj10", SubjectType.Compulsory, grps);
            ScheduleHourContent slot11 = new ScheduleHourContent(11, false, LessonType.Excercise, "teacher10", "room20010", "AA10", "sbj10", SubjectType.Compulsory, grps);
            ScheduleHourContent slot12 = null;
            ScheduleHourContent slot13 = new ScheduleHourContent(13, false, LessonType.Excercise, "teacher13", "room20013", "AA13", "sbj13", SubjectType.Elective, grps);

            day.BlocksInDay.Add(slot1);
            day.BlocksInDay.Add(slot2);
            day.BlocksInDay.Add(slot3);
            day.BlocksInDay.Add(slot4);
            day.BlocksInDay.Add(slot5);
            day.BlocksInDay.Add(slot6);
            day.BlocksInDay.Add(slot7);
            day.BlocksInDay.Add(slot8);
            day.BlocksInDay.Add(slot9);
            day.BlocksInDay.Add(slot10);
            day.BlocksInDay.Add(slot11);
            day.BlocksInDay.Add(slot12);
            day.BlocksInDay.Add(slot13);

            ScheduleWeekContent week = new ScheduleWeekContent();
            week.DaysInWeek.Add(day);
            return week;

        }
    }
}
