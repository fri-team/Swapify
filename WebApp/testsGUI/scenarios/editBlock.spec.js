Feature('Edit block');

Before((I) => {
    I.login();
    I.amOnPage('/timetable');
});

Scenario('[EDIT01] Verify, that modified length of subject will be correctly display on board', async(I) => {

    I.click(locate('.block').withText('5IS208'));
    I.click({ react: 'button' , props: { title : 'Upraviť blok'}});

    I.clearField('length');
    I.fillField('length', '1');

    I.click(locate('span').withText('Uložiť'));

    I.wait(1);

    I.click({ react: 'Block', props: { i: 3, j: 8 }});
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'architektúry informačných systémov',
            courseShortcut: '5IS208',
            day: 3,
            startBlock: 8,
            endBlock: 9,
            room: 'RC009',
            teacher: 'doc. Mgr. Ondrej Šuch PhD.',
            type: 'lecture'
        }
    });

});

Scenario('[EDIT02] Verify, if edited subject is saved (with informations) on board correctly', async(I) => {
    
    I.click(locate('.block').withText('5IL210'));

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

    I.click({ react: 'Block', props: { i: 3, j: 1 }});
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'jazyk anglický 6',
            courseShortcut: '5IL210',
            day: 3,
            startBlock: 1,
            endBlock: 2,
            room: 'RC001',
            teacher: 'Nazov ucitela',
            type: 'excercise'
        }
    });
});

Scenario('[EDIT03] Verify, if lecture will be changed to laboratory', async(I) => {
    I.click({ 
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

    I.click({ react: 'Block', props: { i: 1, j: 9 }});
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
});

Scenario('[EDIT04] Verify, if lenght of subject will not be modified with incorrect value', async(I) => {
    I.click({ 
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
    I.click({ react: 'button' , props: { title : 'Upraviť blok'}});

    I.fillField('startBlock', '07');

    I.clearField('length');
    I.fillField('length', '50');

    let length = I.grabValueFrom('length');
    I.assertEqual('13',(await length).toString());
    I.wait(1);
});