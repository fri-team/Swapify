// const { compareStrings } = require('../helpers');

/*
    To run all tests... :
    $ codeceptjs run

    To run test by ID... :
    $ codeceptjs run --steps --grep "\[FIRST01\]"

    $ codeceptjs run --steps --grep "\[FIRST02\]"

 */

Feature('First-tests');

Before((I) => {
    I.amOnPage('/');
    // reload database?
});

Scenario('[FIRST01] My first test', async (I) => {
    // TODO: login to Swapify
    I.fillField('input[type="email"]', 'Oleg@swapify.com');
    I.fillField('input[type="password"]', 'Heslo123');

    I.click(locate('button').withText('Prihlásiť sa'));
    I.awaitRequests();
    // I.wait(2);
    // I.seeElement(locate('app-container timetable'));
    I.assert(await I.exec(() => !!$('div.timetable')), true);
});
