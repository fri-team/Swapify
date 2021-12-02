Feature('Edit block');

Before((I) => {
    I.login();
    I.amOnPage('/timetable');
});

Scenario('[EDIT01] Verify, that modified length of subject will be correctly display on board', async(I) => {
    I.addBlock(1, 8, 'architektúry informačných systémov', 'doc. Mgr. Ondrej Šuch PhD.','RC009', 'lecture', 2);
    // I.click({ react: 'Block', props: { i: 1, j: 8 }});
    I.click(locate('.block').withText('5IS208'));
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'architektúry informačných systémov',
            courseShortcut: '5IS208',
            day: 1,
            startBlock: 8,
            endBlock: 10,
            room: 'RC009',
            teacher: 'doc. Mgr. Ondrej Šuch PhD.',
            type: 'lecture'
        }
    });

    I.click({ react: 'button' , props: { title : 'Upraviť blok'}});

    I.clearField('length');
    I.fillField('length', '1');
    I.click(locate('span').withText('Uložiť'));

    I.wait(1);

    // I.click({ react: 'Block', props: { i: 1, j: 8 }});
    I.click(locate('.block').withText('5IS208'));
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'architektúry informačných systémov',
            courseShortcut: '5IS208',
            day: 1,
            startBlock: 8,
            endBlock: 9,
            room: 'RC009',
            teacher: 'doc. Mgr. Ondrej Šuch PhD.',
            type: 'lecture'
        }
    });
    I.deleteBlock(1, 8);
});

Scenario('[EDIT02] Verify, if edited subject is saved (with informations) on board correctly', async(I) => { 
    I.addBlock(1, 5, 'jazyk anglický 1', 'Mgr. Lucie Němečková','RA320', 'excercise', 2);
    // I.click({ react: 'Block', props: { i: 1, j: 5 }});
    I.click(locate('.block').withText('5BL115'));
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'jazyk anglický 1',
            courseShortcut: '5BL115',
            day: 1,
            startBlock: 5,
            endBlock: 7,
            room: 'RA320',
            teacher: 'Mgr. Lucie Němečková',
            type: 'excercise'
        }
    });

    I.click({ react: 'button' , props: { title : 'Upraviť blok'}});

    I.clearField('teacher');
    I.fillField('teacher','Nazov ucitela');

    I.clearField('room');
    I.fillField('room', 'RC001');

    I.click('#mui-component-select-day');
    I.click(locate('li').withText('Streda'));

    I.fillField('startBlock', '07');

    I.clearField('length');
    I.fillField('length', '1');

    I.click(locate('span').withText('Uložiť')); 

    I.wait(1);

    // I.click({ react: 'Block', props: { i: 3, j: 1 }});
    I.click(locate('.block').withText('5BL115'));
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'jazyk anglický 1',
            courseShortcut: '5BL115',
            day: 3,
            startBlock: 1,
            endBlock: 2,
            room: 'RC001',
            teacher: 'Nazov ucitela',
            type: 'excercise'
        }
    });
    I.deleteBlock(3,1);
});

Scenario('[EDIT03] Verify, if lecture will be changed to laboratory', async(I) => {
    I.addBlock(1, 9, 'teória informácie', 'doc. RNDr. Stanislav Palúch CSc.','RC009', 'lecture', 2);
    // I.click({ react: 'Block', props: { i: 1, j: 9 }});
    I.click(locate('.block').withText('5IA202'));
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'teória informácie',
            courseShortcut: '5IA202',
            day: 1,
            startBlock: 9,
            endBlock: 11,
            room: 'RC009',
            teacher: 'doc. RNDr. Stanislav Palúch CSc.',
            type: 'lecture'
        }
    });

    I.click({ react: 'button' , props: { title : 'Upraviť blok'}});
    I.click({xpath: "//input[@type='radio'][contains(@value,'laboratory')]"});
    I.click(locate('span').withText('Uložiť'));

    // I.click({ react: 'Block', props: { i: 1, j: 9 }});
    I.click(locate('.block').withText('5IA202'));
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'teória informácie',
            courseShortcut: '5IA202',
            day: 1,
            startBlock: 9,
            endBlock: 11,
            room: 'RC009',
            teacher: 'doc. RNDr. Stanislav Palúch CSc.',
            type: 'laboratory'
        }
    });
    I.deleteBlock(1, 9);
});

Scenario('[EDIT04] Verify, if lenght of subject will not be modified with incorrect value', async(I) => {
    I.addBlock(5, 4, 'architektúry informačných systémov', ' Ing. Ivana Brídová PhD.','RB003', 'laboratory', 2);
    // I.click({ react: 'Block', props: { i: 5, j: 4 }});
    I.click(locate('.block').withText('5IS208'));
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'architektúry informačných systémov',
            courseShortcut: '5IS208',
            day: 5,
            startBlock: 4,
            endBlock: 6,
            room: 'RB003',
            teacher: ' Ing. Ivana Brídová PhD.',
            type: 'laboratory'
        }
    });

    I.click({ react: 'button' , props: { title : 'Upraviť blok'}});

    I.fillField('startBlock', '07');
    I.clearField('length');
    I.fillField('length', '50');

    let length = I.grabValueFrom('length');
    I.assertEqual('13',(await length).toString());
    I.click('.MuiTouchRipple-root');

    // I.deleteBlock(5, 4);
});