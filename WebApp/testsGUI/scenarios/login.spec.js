/* eslint-disable no-undef */
// const { compareStrings } = require('../helpers');

/*
    To run all tests... :
    $ codeceptjs run

    To run test by ID... :
    $ codeceptjs run --steps --grep "\[FIRST01\]"

    $ codeceptjs run --steps --grep "\[FIRST02\]"

 */

Feature('Login');
// TODO: Create test user with unconfirmed email 

Before((I) => {
    I.amOnPage('/');
    // reload database?
});

Scenario('[LOGIN00] Verify if login form contains fields email address, password, reset password link and login button.', async (I) => {
    I.seeElement({ react: 'TextField' , props: { label : 'E-Mailová adresa' , type : 'email' }});
    I.seeElement({ react: 'TextField' , props: { label : 'Heslo' , type : 'password' }});
    I.seeElement(locate('.FormField__Link').withText('Ak si zabudol heslo, klikni na tento link'));
    I.seeElement(locate('.FormField__Button').withText('Prihlásiť sa'));
});

Scenario('[LOGIN01] Valid email, valid password is entered.', async (I) => {
    I.fillField('input[type="email"]', 'Oleg@swapify.com');
    I.fillField('input[type="password"]', 'Heslo123');
    I.click(locate('button').withText('Prihlásiť sa'));
    I.seeElement({ react: 'Timetable' });
});

Scenario('[LOGIN02] Valid email, invalid password is entered.', async (I) => {
    I.fillField('input[type="email"]', 'Oleg@swapify.com');
    I.fillField('input[type="password"]', 'xxxHeslo123');
    I.click(locate('button').withText('Prihlásiť sa'));
    I.see('Zadané heslo nie je správne.');
});

Scenario('[LOGIN03] Invalid email, valid password is entered.', async (I) => {
    I.fillField('input[type="email"]', 'xxxOleg@swapify.com');
    I.fillField('input[type="password"]', 'Heslo123');
    I.click(locate('button').withText('Prihlásiť sa'));
    I.see('Používateľ xxxoleg@swapify.com neexistuje.');
});

Scenario('[LOGIN04] Invalid email, invalid password is entered.', async (I) => {
    I.fillField('input[type="email"]', 'xxxOleg@swapify.com');
    I.fillField('input[type="password"]', 'xxxHeslo123');
    I.click(locate('button').withText('Prihlásiť sa'));
    I.see('Používateľ xxxoleg@swapify.com neexistuje.');
    I.clearField('input[type="email"]');
    I.fillField('input[type="email"]', 'Oleg@swapify.com');
    I.click(locate('button').withText('Prihlásiť sa'));
    I.see('Zadané heslo nie je správne.');
});

Scenario('[LOGIN06] Verify if user is not able to login until he confirms his email address.', async (I) => {
    I.fillField('input[type="email"]', 'Nikita@swapify.com');
    I.fillField('input[type="password"]', 'Heslo123');
    I.click(locate('button').withText('Prihlásiť sa'));
    I.see('Najskôr prosím potvrď svoju emailovú adresu.');
});
