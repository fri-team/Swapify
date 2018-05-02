import { groupByTimeBlocks } from './TimetableBlocks';

describe('groupByTimeBlocks()', () => {
  it('Should return empty array on empty timetable', () => {
    expect(groupByTimeBlocks([])).toEqual([]);
  });

  it('Should group time blocks by its day and hour', () => {
    const timetable = [
      {
        day: 1,
        startBlock: 7,
        endBlock: 9,
        courseShortcut: 'TI',
      },
      {
        day: 2,
        startBlock: 6,
        endBlock: 8,
        courseShortcut: 'TI',
      },
      {
        day: 1,
        startBlock: 4,
        endBlock: 6,
        courseShortcut: 'DISS',
      },
      {
        day: 1,
        startBlock: 7,
        endBlock: 9,
        courseShortcut: 'DISS',
      },
    ];
    // group courses on Monday block 7
    const expected = [
      [
        {
          day: 1,
          startBlock: 4,
          endBlock: 6,
          courseShortcut: 'DISS',
        },
      ],
      [
        {
          day: 1,
          startBlock: 7,
          endBlock: 9,
          courseShortcut: 'DISS',
        },
        {
          day: 1,
          startBlock: 7,
          endBlock: 9,
          courseShortcut: 'TI',
        },
      ],
      [
        {
          day: 2,
          startBlock: 6,
          endBlock: 8,
          courseShortcut: 'TI',
        },
      ],
    ];
    expect(groupByTimeBlocks(timetable)).toEqual(expected);
  });
});
