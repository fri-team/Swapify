using BlazorClient.ViewModels;
using BlazorClient.Models.Timetable;
using System;
using System.Collections.Generic;
using System.Linq;
using BlazorClient.Extensions;

namespace BlazorClient.Services
{
    public class TimetableBlocksConverter
    {
        public TimetableBlocksViewModel CreateTimetableBlocksViewModel(IEnumerable<TimetableBlockModel> blockModels, int rowsCount, int columnsCount)
        {
            var timetableBlocks = CreateTimetableBlockViewModels(blockModels, out var rowHeight);
            return new TimetableBlocksViewModel()
            {
                ColumnsCount = (byte)columnsCount,
                RowsCount = (byte)(rowsCount * rowHeight),
                TimetableBlocks = timetableBlocks
            };
        }

        private static IEnumerable<TimetableBlockViewModel> CreateTimetableBlockViewModels(IEnumerable<TimetableBlockModel> blocks, out int rowHeight)
        {
            // group by day and startBlock
            var blocksGroupedByStart = blocks
                .GroupBy(block => (block.Day, block.StartBlock))
                .OrderBy(group => group.Key);

            // TODO order by day, startBlock, endBlock, course
            rowHeight = blocksGroupedByStart.Select(group => group.Count()).Lcm();

            var blockViewModels = new List<TimetableBlockViewModel>();
            int lastEndBlock = 0;
            int previousGroupDay = 0;
            double marginTop = 0;

            foreach (var group in blocksGroupedByStart)
            {
                lastEndBlock = ResetLastEndBlockIfNewDay(lastEndBlock, previousGroupDay, group.First().Day);
                marginTop = ActualizeMarginTop(marginTop, lastEndBlock, group.First().StartBlock);
                var blockHeight = rowHeight / group.Count();
                foreach (var (block, index) in group.WithIndex())
                {
                    var startLine = (block.Day - 1) * rowHeight + index * blockHeight + 1;
                    blockViewModels.Add(new TimetableBlockViewModel(block)
                    {
                        StartColumn = block.StartBlock,
                        EndColumn = block.EndBlock,
                        StartRow = startLine,
                        EndRow = startLine + blockHeight,
                        MarginTop = marginTop
                    });
                }
                lastEndBlock = ActualizeLastEndBlock(lastEndBlock, group);
                previousGroupDay = group.First().Day;
            }
            return blockViewModels;
        }

        private static int ActualizeLastEndBlock(int lastEndBlock, IGrouping<(int Day, int StartBlock), TimetableBlockModel> blocksGroup)
        {
            var last = lastEndBlock;
            foreach (var block in blocksGroup)
            {
                if (block.EndBlock > last)
                {
                    last = block.EndBlock;
                }
            }
            return last;
        }

        private static int ResetLastEndBlockIfNewDay(int lastEndBlock, int previousDay, int currentDay)
        {
            return currentDay == previousDay ? lastEndBlock : 0;
        }

        private static double ActualizeMarginTop(double marginTop, int lastEndBlock, int currentGroupStartBlock)
        {
            return currentGroupStartBlock < lastEndBlock ? marginTop + 5 : 0;
        }        
    }
}
