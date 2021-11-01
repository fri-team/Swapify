Feature('Delete block');

Before((I) => {
    I.login();
    I.amOnPage('/timetable');
    
});

Scenario('[DELETE01] Verify, if subject with lecture and exercise will be deleted from timetable',async (I) => {
    I.click(locate('.block').withText('5II212'));
    I.click({ react: 'button' , props: { title : 'Vymazať blok'}});
    I.click(locate('.block').withText('5II212'));
    I.click({ react: 'button' , props: { title : 'Vymazať blok'}});
    I.dontSee(locate('.block').withText('5II212'));
});

Scenario('[DELETE02] Verify, if only lecture will be removed and excercise will stay',async (I) => {
    I.wait(2);
    I.addBlock(2, 4, 'teória spoľahlivosti', 'prof. Ing. Elena Zaitseva PhD.', 'RA201', 'laboratory', 2);
    I.wait(2);
    I.addBlock(1, 4, 'teória spoľahlivosti', 'prof. Ing. Elena Zaitseva PhD.', 'RB002', 'lecture', 2);

    I.click(locate('.block').withText('5UI102'));
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'teória spoľahlivosti',
            courseShortcut: '5UI102',
            day: 1,
            startBlock: 4,
            endBlock: 6,
            room: 'RB002',
            teacher: 'prof. Ing. Elena Zaitseva PhD.',
            type: 'lecture'
        }
    });
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
    I.click({ react: 'Block', props: { i: 4, j: 6 }});
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'diskrétna simulácia',
            courseShortcut: '5II208',
            day: 4,
            startBlock: 6,
            endBlock: 8,
            room: 'RB054',
            teacher: ' Ing. Peter Jankovič PhD.',
            type: 'laboratory'
        }
    });
    I.click({ react: 'button' , props: { title : 'Vymazať blok'}});

    I.click({ react: 'Block', props: { i: 4, j: 6 }});
    I.seeElement({ react: 'AddBlockForm' });
    I.click('.MuiTouchRipple-root');

    I.wait(1);

    I.click({ react: 'Block', props: { i: 2, j: 4 }});
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'diskrétna simulácia',
            courseShortcut: '5II208',
            day: 2,
            startBlock: 4,
            endBlock: 6,
            room: 'RC009',
            teacher: 'doc. Ing. Norbert Adamko PhD.',
            type: 'lecture'
        }
    });
});