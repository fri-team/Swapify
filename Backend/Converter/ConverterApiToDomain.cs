using System;
using System.Collections;
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
        public static Timetable MergeSameBlocksWithDifferentTeacher(IEnumerable<Block> blocks)
        {            
            var sortedBlocks = blocks
                    .OrderBy(b => b.Day)
                    .ThenBy(b => b.StartHour)
                    .ThenBy(b => b.Duration)
                    .ThenBy(b => b.Room)
                    .ThenBy(b => b.BlockType)
                    .ThenBy(b => b.CourseId)
                    .ToList();
            
            IEnumerable<Block> mergedBlocks = Merge(sortedBlocks,
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
            foreach (var mergedBlock in mergedBlocks)
            {
                mergedTimetable.AddNewBlock(mergedBlock);
            }            
            return mergedTimetable;
        }
        
        public static async Task<Timetable> ConvertAndMergeSameConsecutiveBlocks(IEnumerable<ScheduleHourContent> blocks, ICourseService courseService, bool isTimetableForCourse)
        {                        
            var sortedBlocks = blocks
                .OrderBy(b => b.Day)
                .ThenBy(b => b.CourseName)                                
                .ThenBy(b => b.TeacherName)
                .ThenBy(b => b.RoomName)
                .ThenBy(b => b.LessonType)
                .ThenBy(b => b.BlockNumber)
                .ToList();

            IEnumerable<Task<Block>> mergedBlocks = MergeAsync(sortedBlocks,
                (group, b2) =>
                {
                    ScheduleHourContent b1 = group.Last();
                    return b1.Day == b2.Day
                        && b1.CourseName == b2.CourseName
                        && b1.TeacherName == b2.TeacherName
                        && b1.RoomName == b2.RoomName
                        && b1.LessonType == b2.LessonType
                        && b1.BlockNumber == b2.BlockNumber - 1;
                },
                async (group) =>
                {
                    ScheduleHourContent firstInGroup = group.First();
                    var block = new Block()
                    {
                        BlockType = ConvertToBlockType(firstInGroup.LessonType),
                        Day = ConvertToDay(firstInGroup.Day),
                        Teacher = firstInGroup.TeacherName,
                        Room = firstInGroup.RoomName,
                        StartHour = (byte)(firstInGroup.BlockNumber + 6), // block number start 1 but starting hour in school is 7:00
                        Duration = (byte)(group.Count)
                    };

                    if (!isTimetableForCourse)
                    {
                        block.CourseId = await courseService.GetOrAddNotExistsCourseId(firstInGroup.CourseName, block);
                    }

                    return block;
                }                
            );

            Timetable mergedTimetable = new Timetable();
            foreach (var mergedBlock in mergedBlocks)
            {
                mergedTimetable.AddNewBlock(await mergedBlock);
            }

            return mergedTimetable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TMerged"></typeparam>
        /// <param name="sortedElements">Order has to be such that elements from same group are in list together.</param>
        /// <param name="isInGroup">If next element from sortedList is in the group.</param>
        /// <param name="mergeElementsGroup">Merge group of elements to one element.</param>
        /// <returns></returns>
        public static IEnumerable<Task<TMerged>> MergeAsync<TElement, TMerged>(
            IEnumerable<TElement> sortedElements,
            Func<List<TElement>, TElement, bool> isInGroup,
            Func<List<TElement>, Task<TMerged>> mergeElementsAsync)
        {
            List<TElement> elementsGroup = new List<TElement>();

            foreach (var element in sortedElements)
            {
                if (elementsGroup.Count == 0 || isInGroup(elementsGroup, element))
                {
                    elementsGroup.Add(element);
                }
                else
                {
                    yield return mergeElementsAsync(elementsGroup);
                    elementsGroup.Clear();
                    elementsGroup.Add(element);
                }
            }

            if (elementsGroup.Count > 0)
            {
                yield return mergeElementsAsync(elementsGroup);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortedElements">Order has to be such that elements from same group are in list together.</param>
        /// <param name="inSameGroup">If next element from sortedList and last element from actual group are in same group.</param>
        /// <param name="mergeElementsGroup">Merge group of elements to one element.</param>
        /// <returns></returns>
        public static IEnumerable<T> Merge<T>(IEnumerable<T> sortedElements, Func<T,T,bool> inSameGroup, Func<List<T>, T> mergeElementsGroup)
        {            
            List<T> elementsGroup = new List<T>();            
            
            foreach (var element in sortedElements)
            {
                if (elementsGroup.Count == 0 || inSameGroup(elementsGroup[elementsGroup.Count - 1], element))
                {
                    elementsGroup.Add(element);                    
                }
                else
                {
                    yield return mergeElementsGroup(elementsGroup);
                    elementsGroup.Clear();
                    elementsGroup.Add(element);
                }                
            }

            if (elementsGroup.Count > 0)
            {
                yield return mergeElementsGroup(elementsGroup);
            }            
        }
        
        public static async Task<Timetable> ConvertTimetableForGroupAsync(IEnumerable<ScheduleHourContent> groupTimetable, ICourseService courseServ)
        {
            return await ConvertAndMergeSameConsecutiveBlocks(groupTimetable, courseServ, false);            
        }
        
        public static async Task<Timetable> ConvertTimetableForCourseAsync(IEnumerable<ScheduleHourContent> courseTimetable, ICourseService courseServ)
        {
            return await ConvertAndMergeSameConsecutiveBlocks(courseTimetable, courseServ, true);            
        }

        // TODO rewrite
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
