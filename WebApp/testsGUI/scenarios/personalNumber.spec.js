/* eslint-disable no-undef */

Feature('Personal number change');

Before((I) => {
    I.login();
    I.amOnPage('/timetable');
});

Scenario('[PENUM01] Verify, if change of personal number (existing) will bring student timetable (there is at lest one block).', async (I) => {
    I.click(locate('.cursor'));
    I.click('Zmeniť číslo');
    I.see('Zadajte osobné číslo');
    I.seeElement({ react: 'TextField' , props: { id : 'personalNumber' }});
    I.fillField('Zadajte osobné číslo', '558888');
    I.click('Uložiť');
    I.wait(3);
    I.seeElement({ react: 'Timetable' });
    I.seeElement(locate('.block'));
});

Scenario('[PENUM02] Verify, if change of personal number (not existing) will show error.', async (I) => {
    I.click(locate('.cursor'));
    I.click('Zmeniť číslo');
    I.see('Zadajte osobné číslo');
    I.seeElement({ react: 'TextField' , props: { id : 'personalNumber' }});
    I.fillField('Zadajte osobné číslo', '000000');
    I.click('Uložiť');
    I.wait(3);
    I.see('Neexistujúce osobné číslo');
});