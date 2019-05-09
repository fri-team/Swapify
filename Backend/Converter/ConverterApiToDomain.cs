using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FRITeam.Swapify.APIWrapper.Enums;
using FRITeam.Swapify.APIWrapper.Objects;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Entities;
using FRITeam.Swapify.Entities.Enums;
using Org.BouncyCastle.Apache.Bzip2;

namespace FRITeam.Swapify.Backend.Converter
{
    public static class ConverterApiToDomain
    {
        public static Timetable MergeSameBlocksWithDifferentTeacher(Timetable timetable)
        {            
            var sortedBlocks =
                timetable.AllBlocks
                    .OrderBy(b => b.Day)
                    .ThenBy(b => b.StartHour)
                    .ThenBy(b => b.Duration)
                    .ThenBy(b => b.Room)
                    .ThenBy(b => b.BlockType)
                    .ThenBy(b => b.CourseId)
                    .ToList();
            
            List<Block> mergedBlocks = Merge(sortedBlocks,
                (b1, b2) => b1.Day == b2.Day
                            && b1.StartHour == b2.StartHour
                            && b1.Duration == b2.Duration
                            && b1.Room == b2.Room
                            && b1.BlockType == b2.BlockType
                            && b1.CourseId == b2.CourseId,
                (blocksGroup) =>
                {
                    Block block = blocksGroup[0].Clone();
                    block.Teacher = string.Join(",", blocksGroup.Select(b => b.Teacher));
                    return block;
                }
            );

            Timetable mergedTimetable = new Timetable();
            mergedBlocks.ForEach(b => mergedTimetable.AddNewBlock(b));            
            return mergedTimetable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortedElements">Order has to be such that elements from same group are in list together.</param>
        /// <param name="inSameGroup">If next element from sortedList and last element from actual group are in same group.</param>
        /// <param name="mergeElementsGroup">Merge group of elements to one element.</param>
        /// <returns></returns>
        public static List<T> Merge<T>(List<T> sortedElements, Func<T,T,bool> inSameGroup, Func<List<T>, T> mergeElementsGroup)
        {            
            List<T> elementsGroup = new List<T>();
            List<T> mergedElements = new List<T>();
            
            foreach (var element in sortedElements)
            {
                if (elementsGroup.Count == 0 || inSameGroup(elementsGroup[elementsGroup.Count - 1], element))
                {
                    elementsGroup.Add(element);                    
                }
                else
                {
                    mergedElements.Add(mergeElementsGroup(elementsGroup));
                    elementsGroup.Clear();
                    elementsGroup.Add(element);
                }                
            }

            if (elementsGroup.Count > 0)
            {
                mergedElements.Add(mergeElementsGroup(elementsGroup));
            }

            return mergedElements;
        }

        public static async Task<Timetable> ConvertTimetableForGroupAsync(ScheduleWeekContent groupTimetable, ICourseService courseServ)
        {
            return await ConvertTimetableAsync(groupTimetable, courseServ, false);
        }

        public static async Task<Timetable> ConvertTimetableForCourseAsync(ScheduleWeekContent courseTimetable, ICourseService courseServ)
        {
            return await ConvertTimetableAsync(courseTimetable, courseServ, true);
        }

        // TODO handle blocks with same time, place and course
        private static async Task<Timetable> ConvertTimetableAsync(ScheduleWeekContent schedule, ICourseService courseServ, bool isTimetableForCourse)
        {
            Timetable timetable = new Timetable();
            for (int idxDay = 0; idxDay < schedule.DaysInWeek.Count; idxDay++)
            {
                var maxBlocks = schedule.DaysInWeek[idxDay].BlocksInDay.Count;

                byte startingBlock = 0;
                for (int blckIdx = 1; blckIdx < maxBlocks; blckIdx++)
                {
                    var blockBefore = schedule.DaysInWeek[idxDay].BlocksInDay[blckIdx - 1];
                    var block = schedule.DaysInWeek[idxDay].BlocksInDay[blckIdx];
                    if (blockBefore == null)
                    {
                        startingBlock = (byte)blckIdx;

                        if (block != null && blckIdx == maxBlocks - 1)
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
                            if (!isTimetableForCourse)
                            {
                                bl.CourseId = await courseServ.GetOrAddNotExistsCourseId(block.CourseName, bl);
                            }
                            timetable.AddNewBlock(bl);
                        }
                        continue;
                    }
                    if (!blockBefore.IsSameBlockAs(block))
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
                        if (!isTimetableForCourse)
                        {
                            bl.CourseId = await courseServ.GetOrAddNotExistsCourseId(blockBefore.CourseName, bl);
                        }

                        timetable.AddNewBlock(bl);
                        startingBlock = (byte)blckIdx;
                    }
                }
            }
            return timetable;
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
                case LessonType.Laboratory:
                    return BlockType.Laboratory;
                case LessonType.Lecture:
                    return BlockType.Lecture;
                default:
                    throw new Exception("Unknow LessonType");
            }
        }
    }
}
