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
        courseCode: 'TI',
      },
      {
        day: 2,
        startBlock: 6,
        endBlock: 8,
        courseCode: 'TI',
      },
      {
        day: 1,
        startBlock: 4,
        endBlock: 6,
        courseCode: 'DISS',
      },
      {
        day: 1,
        startBlock: 7,
        endBlock: 9,
        courseCode: 'DISS',
      },
    ];
    // group courses on Monday block 7
    const expected = [
      [
        {
          day: 1,
          startBlock: 4,
          endBlock: 6,
          courseCode: 'DISS',
        },
      ],
      [
        {
          day: 1,
          startBlock: 7,
          endBlock: 9,
          courseCode: 'DISS',
        },
        {
          day: 1,
          startBlock: 7,
          endBlock: 9,
          courseCode: 'TI',
        },
      ],
      [
        {
          day: 2,
          startBlock: 6,
          endBlock: 8,
          courseCode: 'TI',
        },
      ],
    ];
    expect(groupByTimeBlocks(timetable)).toEqual(expected);
  });
});
