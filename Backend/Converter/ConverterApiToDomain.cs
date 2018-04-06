using System;
using System.Collections.Generic;
using System.Text;
using Backend.Database;
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
        public static Timetable GetStudyGroupTimetable(ScheduleWeekContent schedule)
        {
            
            Timetable timetable = new Timetable();
            
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
                            timetable.Blocks.Add(new Block()
                            {
                                BlockType = ConvertToBlockType(block.LessonType),
                                Day = ConvertToDay(idxDay),
                                Teacher = block.TeacherName,
                                Room = block.RoomName,
                                StartHour = (byte)(schedule.DaysInWeek[idxDay].BlocksInDay[blckIdx].BlockNumber+6),
                                Duration = 1
                            });
                        }
                        continue;
                        
                    } 
                    if (!IsSameBlock(blockBefore, block))
                    {
                        timetable.Blocks.Add(new Block()
                        {
                            BlockType = ConvertToBlockType(blockBefore.LessonType),
                            Day = ConvertToDay(idxDay),
                            Teacher = blockBefore.TeacherName,
                            Room = blockBefore.RoomName,
                            StartHour = (byte)(schedule.DaysInWeek[idxDay].BlocksInDay[startingBlock].BlockNumber + 6),
                            Duration = (byte)(blckIdx - startingBlock),
                       //     CourseId = GetOrCreateCourseIdFromBlock(blockBefore.CourseName)
                            
                        });
                        startingBlock = (byte)blckIdx;
                    }


                }
                idxDay++;
            }

            return timetable;
        }


        //private static Guid GetOrCreateCourseIdFromBlock(string courseName)
        //{
        //    ICourseService courseServ = new CourseService(DBSettings.Database);
        //    var course = courseServ.FindByNameAsync(courseName);
        //    if (course == null)
        //    {
        //        courseServ.AddAsync(new Course() { CourseName = courseName },);
        //    }
        //    return null;
        //}

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
