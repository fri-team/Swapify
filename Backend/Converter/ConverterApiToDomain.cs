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
                (group, b2) =>
                {
                    Block b1 = group.Last();
                    return b1.Day == b2.Day
                           && b1.StartHour == b2.StartHour
                           && b1.Duration == b2.Duration
                           && b1.Room == b2.Room
                           && b1.BlockType == b2.BlockType
                           && b1.CourseId == b2.CourseId;
                },
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

            IEnumerable<Task<Block>> mergedBlocks = Merge(sortedBlocks,
                (group, b2) =>
                {
                    ScheduleHourContent b1 = group.Last();
                    return b1.Day == b2.Day
                        && b1.CourseName == b2.CourseName
                        && b1.TeacherName == b2.TeacherName
                        && b1.RoomName == b2.RoomName
                        && b1.LessonType == b2.LessonType
                        && (b1.BlockNumber == b2.BlockNumber - 1
                            || b1.BlockNumber == b2.BlockNumber); // || b1.BlockNumber == b2.BlockNumber to eliminate duplicates (same blocks) 
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
                        Duration = (byte)(group.Last().BlockNumber - firstInGroup.BlockNumber + 1)
                    };

                    if (!isTimetableForCourse)
                    {
                        Course course = null;
                        if (!string.IsNullOrEmpty(firstInGroup.CourseShortcut))
                        {
                        	course = await courseService.GetOrAddNotExistsCourseByShortcut(firstInGroup.CourseShortcut, firstInGroup.CourseName);   
                        }
                        else
                        {                                                        
                            course = await courseService.GetOrAddNotExistsCourseByName(firstInGroup.CourseName, firstInGroup.CourseShortcut);                            
                        }
                        if (course == null)
                        {
                            throw new Exception("Course has no name and no Shortcut, therefore it could not be added.");
                        }
                        Block courseBlock = course.Timetable.GetBlock(block);
                        if (courseBlock != null)
                            block.BlockId = courseBlock.BlockId;

                        block.CourseId = course.Id;
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
        /// <typeparam name="TResult"></typeparam>
        /// <param name="sortedElements">Order has to be such that elements from same group are in list together.</param>
        /// <param name="isInGroup">If next element from sortedList is in the group.</param>
        /// <param name="mergeElementsGroup">Merge group of elements to one element.</param>
        /// <returns></returns>
        public static IEnumerable<TResult> Merge<TElement, TResult>(
            IEnumerable<TElement> sortedElements,
            Func<List<TElement>, TElement, bool> isInGroup,
            Func<List<TElement>, TResult> mergeElementsGroup)
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

        public static async Task<Timetable> ConvertTimetableForPersonalNumberAsync(IEnumerable<ScheduleHourContent> timetable, ICourseService courseServ)
        {
            return await ConvertAndMergeSameConsecutiveBlocks(timetable, courseServ, false);
        }
        
        public static async Task<Timetable> ConvertTimetableForCourseAsync(IEnumerable<ScheduleHourContent> timetable, ICourseService courseServ)
        {
            return await ConvertAndMergeSameConsecutiveBlocks(timetable, courseServ, true);            
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
