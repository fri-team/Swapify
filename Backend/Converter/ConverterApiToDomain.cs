using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.APIWrapper.Enums;
using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;

namespace FRITeam.Swapify.Backend.Converter
{
    public class ConverterApiToDomain
    {

        /// <summary>
        /// Return timetable and list of courses numbers
        /// </summary>
        public static Tuple<Timetable,List<string>> ConvertTimetable(ScheduleWeekContent schedule)
        {
            
            Timetable timetable = new Timetable();
            List<string> coursesNumbers = new List<string>();
            for (int idxDay = 0;idxDay < schedule.DaysInWeek.Count;idxDay++)
            {
                var maxBlocks = schedule.DaysInWeek[idxDay].BlocksInDay.Count;

                byte startingBlock = 0;
                for (int blckIdx = 1;blckIdx < maxBlocks;blckIdx++)
                {
                    var blockBefore = schedule.DaysInWeek[idxDay].BlocksInDay[blckIdx-1];
                    var block = schedule.DaysInWeek[idxDay].BlocksInDay[blckIdx];
                    if (blockBefore == null)
                    {
                        startingBlock = (byte)blckIdx;

                        if (block != null && blckIdx == maxBlocks-1)
                        {
                            var bl = new Block()
                            {
                                BlockType = ConvertToBlockType(block.LessonType),
                                Day = ConvertToDay(idxDay),
                                Teacher = block.TeacherName,
                                Room = block.RoomName,
                                StartHour = (byte)(schedule.DaysInWeek[idxDay].BlocksInDay[blckIdx].BlockNumber + 6),
                                Duration = 1
                            };
                            
                            coursesNumbers.Add(block.CourseName);
                            
                            timetable.Blocks.Add(bl);
                        }
                        continue;
                        
                    } 
                    if (!IsSameBlock(blockBefore, block))
                    {
                        var bl = new Block()
                        {
                            BlockType = ConvertToBlockType(blockBefore.LessonType),
                            Day = ConvertToDay(idxDay),
                            Teacher = blockBefore.TeacherName,
                            Room = blockBefore.RoomName,
                            StartHour = (byte)(schedule.DaysInWeek[idxDay].BlocksInDay[startingBlock].BlockNumber + 6), //blocknumber start 1 but starting hour in school is 7:00
                            Duration = (byte)(blckIdx - startingBlock)
                        };
                        coursesNumbers.Add(blockBefore.CourseName);

                        timetable.Blocks.Add(bl);
                        startingBlock = (byte)blckIdx;
                    }


                }
                idxDay++;
            }

            return new Tuple<Timetable, List<string>>(timetable,coursesNumbers);
        }


        public static async Task<Tuple<List<Guid>,List<string>>> GetOrCreateCourseIdFromBlock(List<string> courses,
                                                                                        ICourseService courseServ,
                                                                                        ISchoolScheduleProxy proxy)
        {
            List<Guid> courseIds = new List<Guid>();
            foreach (var courseName in courses)
            {
                var course = await courseServ.FindByNameAsync(courseName);
                if (course == null)
                {
                    var downloadedTimetable = proxy.GetBySubjectCode(courseName);
                    var converted = ConvertTimetable(downloadedTimetable);
                    course = new Course() { CourseName = courseName, Timetable = converted.Item1 };
                    await courseServ.AddAsync(course);
                }
                courseIds.Add(course.Id);
            }
            return new Tuple<List<Guid>, List<string>>(courseIds,null);
        }

        private static bool IsSameBlock(ScheduleHourContent b1, ScheduleHourContent b2)
        {
            return (b1.CourseName == b2?.CourseName) &&
                    (b1.TeacherName == b2?.TeacherName) &&
                    (b1.RoomName == b2?.RoomName) &&
                    (b1.LessonType == b2?.LessonType) &&
                    (b1.StudyGroups.SetEquals(b2?.StudyGroups));
        }

        private static Day ConvertToDay(int idxDay)
        {
            switch (idxDay)
            {
                case 0:
                    return Day.Monday;
                case 1:
                    return Day.Tuesday;
                case 2:
                    return Day.Wednesday;
                case 3:
                    return Day.Thursday;
                case 4:
                    return Day.Friday;
                default:
                    throw new Exception("Unknow Day");
            }
        }

        private static BlockType ConvertToBlockType(LessonType type)
        {
            switch (type)
            {
                case LessonType.Excercise:
                    return BlockType.Excercise;
                case LessonType.Laboratory :
                    return BlockType.Laboratory;
                case LessonType.Lecture:
                    return BlockType.Lecture;
                default:
                    throw new Exception("Unknow LessonType");
            }

        }
    }
}
