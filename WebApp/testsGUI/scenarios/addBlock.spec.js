/* eslint-disable no-undef */

Feature('Add block features');

Before((I) => {
    I.login();
    I.amOnPage('/timetable');
});

Scenario('[ADD00] Check, if click on empty field open up from for adding.', async (I) => {
    I.click({ react: 'Block', props: { i: 1, j: 1 }});
    I.seeElement({ react: 'AddBlockForm' });
});

Scenario('[ADD01] Add new block.', async (I) => {
    I.click({ react: 'Block', props: { i: 1, j: 1 }});
    I.fillField('courseName', 'Informatika');
    I.wait(1);
    I.pressKey("ArrowDown");
    I.pressKey("Enter");
    I.fillField('teacher', 'Ján Janech');
    I.fillField('room', 'RA013');
    I.click({ react: 'FormControlLabel', props: { value: 'excercise' }});
    I.click('Uložiť');
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'informatika 1',
            courseCode: '11M151',
            day: 1,
            startBlock: 1,
            endBlock: 3,
            room: 'RA013',
            teacher: 'Ján Janech',
            type: 'excercise'
        }
    });
});

Scenario('[ADD02] Add new block with edited attributes.', async (I) => {
    I.click({ react: 'Block', props: { i: 2, j: 2 }});
    I.fillField('courseName', 'algebra');
    I.wait(2);
    I.pressKey("ArrowDown");
    I.pressKey("Enter");
    I.fillField('teacher', 'Ida Stankovianska');
    I.fillField('room', 'RB053');
    I.click({ react: 'Select', props: { name: 'day' }});
    I.click({ react: 'ButtonBase', props: { 'data-value' : 3 }});
    I.updateField({ react: 'Input', props: { name: 'startBlock' }}, '7');
    I.updateField({ react: 'Input', props: { name: 'length' }}, '3');
    I.click({ react: 'FormControlLabel', props: { value: 'lecture' }});
    I.click('Uložiť');
    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'algebra',
            courseCode: '5BF101',
            day: 3,
            startBlock: 1,
            endBlock: 4,
            room: 'RB053',
            teacher: 'Ida Stankovianska',
            type: 'lecture'
        }
    });
});

Scenario('[ADD03] Check, if subject is founded without accent', async (I) => {
    I.click({ react: 'Block', props: { i: 3, j: 3 }});
    I.fillField('courseName', 'architektury informacnych systemov');
    I.pressKey("ArrowDown");
    I.pressKey("Enter");
    I.seeInField('courseName', 'architektúry informačných systémov (5IS208)');
});

Scenario('[ADD04] Check, if subject is founded with accent', async (I) => {
    I.click({ react: 'Block', props: { i: 3, j: 3 }});
    I.fillField('courseName', 'číslicové počítače');
    I.pressKey("ArrowDown");
    I.pressKey("Enter");
    I.seeInField('courseName', 'číslicové počítače (5BH118)');
});

Scenario('[ADD05] Check, if founded subjects is sorted by faculty of student', async (I) => {
    I.click({ react: 'Block', props: { i: 3, j: 3 }});
    I.fillField('courseName', 'architektury');
    I.pressKey("ArrowDown");
    I.pressKey("Enter");
    I.seeInField('courseName', 'architektúry informačných systémov (5IS208)');

    let first = (await I.grabValueFrom('courseName')).toString().split('(')[1].charAt(0);

    I.clearField('courseName');
    I.fillField('courseName', 'architektury');

    I.wait(1);

    I.pressKey("ArrowDown");
    I.pressKey("ArrowDown");
    I.pressKey("Enter");
    I.seeInField('courseName', 'architektúry zariadení (8KB048)');

    let second = (await I.grabValueFrom('courseName')).toString().split('(')[1].charAt(0);

    if (first <= second)
        I.assertOk(first, 'Ok');
    else
        I.assertFail(second, 'Fail');
    
});

//Test requires actual schedule of subject
Scenario('[ADD06] Check, if forms in add block form will pre-fill', async (I) => {
    I.click({ react: 'Block', props: { i: 3, j: 4 }});
    I.fillField('courseName', 'architektury informacnych systemov');
    I.pressKey("ArrowDown");
    I.pressKey("Enter");

    I.seeInField('teacher', ' Ing. Ivana Brídová PhD.');
    I.seeInField('room', 'RB003');
    I.seeInField('length', '2');
    I.seeElement({ react: 'FormControlLabel', props: { value: 'laboratory' }});
});

Scenario('[ADD07] Add subject for summer semester from sidebar', async (I) => {
    I.click(locate('.MuiIconButton-label'));
    I.click(locate('button').withAttr({ title: 'Pridať predmet' }));

    I.fillField('courseName', 'metaheuristiky');

    I.pressKey("ArrowDown");
    I.pressKey("Enter");

    I.click("Vyhľadať");

    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'metaheuristiky',
        }
    });

    I.click({ 
        react: 'TimetableBlock', props: { 
            courseName : 'metaheuristiky',
        }
    });
    
    I.click({ 
        react: 'TimetableBlock', props: { 
            courseName : 'metaheuristiky',
        }
    });

    I.click({ react: 'button' , props: { title : 'Pridať blok'}});

    I.seeElement({ 
        react: 'TimetableBlock', props: { 
            courseName : 'metaheuristiky',
        }
    });

    I.click(locate('.MuiIconButton-label'));
    
    I.seeElement(locate('label').withText('metaheuristiky'));
});