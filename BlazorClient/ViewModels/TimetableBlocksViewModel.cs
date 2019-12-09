using System.Collections.Generic;

namespace BlazorClient.ViewModels
{
    public class TimetableBlocksViewModel
    {
        public IEnumerable<TimetableBlockViewModel> TimetableBlocks { get; set; }
        public byte ColumnsCount { get; set; }
        public byte RowsCount { get; set; }
    }
}
