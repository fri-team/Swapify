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