Feature('Delete block');

Before((I) => {
    I.login();
    I.amOnPage('/timetable');
    
});

Scenario('[DELETE01] Verify, if subject will be deleted from timetable',async (I) => {
    I.addBlock(2, 4, 'teória spoľahlivosti', 'prof. Ing. Elena Zaitseva PhD.', 'RA201', 'laboratory', 2);
    I.click(locate('.block').withText('5UI102'));
    I.click({ react: 'button' , props: { title : 'Vymazať blok'}});

    I.dontSee(locate('.block').withText('5UI102'));
});

Scenario('[DELETE02] Verify, if only lecture will be removed and excercise will stay',async (I) => {
    I.wait(1);
    I.addBlock(2, 4, 'teória spoľahlivosti', 'prof. Ing. Elena Zaitseva PhD.', 'RA201', 'laboratory', 2);
    I.wait(1);
    I.addBlock(1, 4, 'teória spoľahlivosti', 'prof. Ing. Elena Zaitseva PhD.', 'RB002', 'lecture', 2);

    I.click({ react: 'Block', props: { i: 1, j: 4 }});
    I.click({ react: 'button' , props: { title : 'Vymazať blok'}});

    I.click({ react: 'Block', props: { i: 1, j: 4 }});
    I.seeElement({ react: 'AddBlockForm' });

    I.click('.MuiTouchRipple-root');

    I.wait(1);

    I.click({ react: 'Block', props: { i: 2, j: 4 }});
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'teória spoľahlivosti',
            courseShortcut: '5UI102',
            day: 2,
            startBlock: 4,
            endBlock: 6,
            room: 'RA201',
            teacher: 'prof. Ing. Elena Zaitseva PhD.',
            type: 'laboratory'
        }
    });

    I.deleteBlock(2, 4);
});

Scenario('[DELETE03] Verify, if only exercise will be removed and lecture will stay',async (I) => {
    I.wait(1);
    I.addBlock(2, 4, 'teória spoľahlivosti', 'prof. Ing. Elena Zaitseva PhD.', 'RA201', 'laboratory', 2);
    I.wait(1);
    I.addBlock(1, 4, 'teória spoľahlivosti', 'prof. Ing. Elena Zaitseva PhD.', 'RB002', 'lecture', 2);

    I.click({ react: 'Block', props: { i: 2, j: 4 }});
    I.click({ react: 'button' , props: { title : 'Vymazať blok'}});

    I.click({ react: 'Block', props: { i: 2, j: 4 }});
    I.seeElement({ react: 'AddBlockForm' });
    I.click('.MuiTouchRipple-root');

    I.wait(1);

    I.click({ react: 'Block', props: { i: 1, j: 4 }});
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'teória spoľahlivosti',
            courseShortcut: '5UI102',
            day: 1,
            startBlock: 4,
            endBlock: 6,
            room: 'RA201',
            teacher: 'prof. Ing. Elena Zaitseva PhD.',
            type: 'lecture'
        }
    });
});