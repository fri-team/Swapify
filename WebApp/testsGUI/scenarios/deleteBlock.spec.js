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
    let nameOfDeletedSubject = (await I.grabTextFrom(locate('.block').withText('5UI102').at(1))).toString().substr(0,6);
    
    I.click(locate('.block').withText('5UI102').at(1));
    I.dontSeeElement({ react: 'button' , props: { title : 'Požiadať o výmenu'}});
    I.click({ react: 'button' , props: { title : 'Vymazať blok'}});

    I.seeElement(locate('.block').withText(nameOfDeletedSubject));
});

Scenario('[DELETE03] Verify, if only exercise will be removed and lecture will stay',async (I) => {
    let nameOfDeletedSubject = (await I.grabTextFrom(locate('.block').withText('5II208').at(2))).toString().substr(0,6);

    I.click(locate('.block').withText('5II208').at(2));
    I.seeElement({ react: 'button' , props: { title : 'Požiadať o výmenu'}});
    I.click({ react: 'button' , props: { title : 'Vymazať blok'}});

    I.seeElement(locate('.block').withText(nameOfDeletedSubject));
});