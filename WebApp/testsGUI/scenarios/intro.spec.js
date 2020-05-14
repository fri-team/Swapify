/* eslint-disable no-undef */

Feature('Introduction');

Before((I) => {
    I.amOnPage('/');
});

Scenario('[INTRO00] Verify, if introduction page contains logo, tab menu, login form and about us page link.', async (I) => {
    I.seeElement(locate('.logo'));
    I.seeElement(locate('.PageSwitcher'));
    I.seeElement({ react: 'TextField' , props: { label : 'E-Mailová adresa' , type : 'email' }});
    I.seeElement({ react: 'TextField' , props: { label : 'Heslo' , type : 'password' }});
    I.seeElement(locate('.FormField__Link').withText('Ak si zabudol heslo, klikni na tento link'));
    I.seeElement(locate('.FormField__Button').withText('Prihlásiť sa'));
    I.seeElement(locate('.FormField__Link').withText('O nás'));
});