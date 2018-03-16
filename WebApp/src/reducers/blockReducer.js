const initState = {
  colHeadings: ['07:00', '08:00', '09:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00', '17:00', '18:00', '19:00'],
  rowHeadings: ['Po', 'Ut', 'St', 'Št', 'Pi'],
  blocks: [
    {
      day: 1,
      startBlock: 7,
      endBlock: 9,
      name: 'TI',
      room: 'RA301',
      tearcher: 'Tomáš Majer',
      type: 'laboratory',
    },
    {
      day: 2,
      startBlock: 6,
      endBlock: 8,
      name: 'TI',
      room: 'RC009',
      tearcher: 'Stanislav Palúch',
      type: 'lecture',
    },
    {
      day: 3,
      startBlock: 8,
      endBlock: 10,
      name: 'AIS',
      room: 'RB003',
      tearcher: 'Matilda Drozdová',
      type: 'laboratory',
    },
    {
      day: 4,
      startBlock: 1,
      endBlock: 3,
      name: 'II05',
      room: 'RC009',
      tearcher: 'Vitaly Levashenko',
      type: 'lecture',
    },
    {
      day: 4,
      startBlock: 6,
      endBlock: 8,
      name: 'AIS',
      room: 'RC009',
      tearcher: 'Matilda Drozdová',
      type: 'lecture',
    },
    {
      day: 4,
      startBlock: 9,
      endBlock: 11,
      name: 'TSP',
      room: 'RA201',
      tearcher: 'Elena Zaitseva',
      type: 'laboratory',
    },
    {
      day: 4,
      startBlock: 11,
      endBlock: 13,
      name: 'DISS',
      room: 'AF3A6',
      tearcher: 'Norbert Adamko',
      type: 'lecture',
    },
    {
      day: 5,
      startBlock: 2,
      endBlock: 4,
      name: 'TSP',
      room: 'RA201',
      tearcher: 'Elena Zaitseva',
      type: 'lecture',
    },
    {
      day: 5,
      startBlock: 4,
      endBlock: 6,
      name: 'DISS',
      room: 'RB054',
      tearcher: 'Boris Bučko',
      type: 'laboratory',
    },
    {
      day: 5,
      startBlock: 6,
      endBlock: 8,
      name: 'II05',
      room: 'RB052',
      tearcher: 'Vitaly Levashenko',
      type: 'laboratory',
    },
    {
      day: 1,
      startBlock: 7,
      endBlock: 9,
      name: 'DISS',
      room: 'RA301',
      tearcher: 'Split It',
      type: 'exercise',
    },
  ],
};

export default function fuelSavingsReducer(state = initState, { type }) {
  switch (type) {
    default:
      return state;
  }
}
