/* eslint-disable no-undef */

Feature('Registration');

Before((I) => {
    I.amOnPage('/register');
});

Scenario('[REGIS00] Verify, if registration form contains name field, surname field, mail address field, password field, confirm pasword field, Privacy policy acceptation checkbox and register button.', async (I) => {
    I.seeElement({ react: 'TextField' , props: { label : 'Meno' , type : 'text' }});
    I.seeElement({ react: 'TextField' , props: { label : 'Priezvisko' , type : 'text' }});
    I.seeElement({ react: 'TextField' , props: { label : 'E-Mailová adresa' , type : 'email' }});
    I.seeElement({ react: 'TextField' , props: { label : 'Heslo' , type : 'password' }});
    I.seeElement({ react: 'TextField' , props: { label : 'Potvrdenie hesla' , type : 'password' }});
    I.seeElement('input[type=checkbox]');
    I.seeElement(locate('.FormField__Button').withText('Registrovať sa'));
});
