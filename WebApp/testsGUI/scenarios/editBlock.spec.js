Feature('Edit block');

Before((I) => {
    I.login();
    I.amOnPage('/timetable');
});

Scenario('[EDIT01] Verify, that modified length of subject will be correctly display on board', async(I) => {

    I.click(locate('.block')
        .withAttr({style: 'grid-area: 3 / 4 / 5 / 6; margin-top: 0%; background-color: rgb(255, 87, 34); color: rgb(255, 255, 255); z-index: 2;'}));
    I.click({ react: 'button' , props: { title : 'Upraviť blok'}});

    I.clearField('length');
    I.fillField('length', '1');

    I.click(locate('span').withText('Uložiť'));

    I.seeElement(locate('.block')
        .withAttr({style: 'grid-area: 3 / 4 / 5 / 5; margin-top: 0%; background-color: rgb(255, 87, 34); color: rgb(255, 255, 255); z-index: 2;'}));
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

    let styleAndPositionOfEditedBlock = 'grid-area: 5 / 1 / 7 / 2; margin-top: 0%; background-color: rgb(0, 188, 212); color: rgba(0, 0, 0, 0.87); z-index: 2;';
    I.seeElement(locate('.block')
        .withAttr({style: styleAndPositionOfEditedBlock})
        .withChild('.teacher').withText('Nazov ucitela'));
    
    I.seeElement(locate('.block')
        .withAttr({style: styleAndPositionOfEditedBlock})
        .withChild(locate('div').withChild('.name').withText('5IL210')));
    
    I.seeElement(locate('.block')
        .withAttr({style: styleAndPositionOfEditedBlock})
        .withChild('.room').withText('RC001'));

});

Scenario('[EDIT03] Verify, if lecture will be changed to laboratory', async(I) => {
    I.click(locate('.block').at(2));
    I.click({ react: 'button' , props: { title : 'Upraviť blok'}});

    I.click({xpath: "//input[@type='radio'][contains(@value,'laboratory')]"});
    I.click(locate('span').withText('Uložiť'));

    let iconsOfBlocks = await I.grabAttributeFrom(locate('.block div svg path'), 'd'); 
    I.assertEqual(iconsOfBlocks[1],'M437.2 403.5L320 215V64h8c13.3 0 24-10.7 24-24V24c0-13.3-10.7-24-24-24H120c-13.3 0-24 10.7-24 24v16c0 13.3 10.7 24 24 24h8v151L10.8 403.5C-18.5 450.6 15.3 512 70.9 512h306.2c55.7 0 89.4-61.5 60.1-108.5zM137.9 320l48.2-77.6c3.7-5.2 5.8-11.6 5.8-18.4V64h64v160c0 6.9 2.2 13.2 5.8 18.4l48.2 77.6h-172z');

    I.click(locate('.block').at(2));
    I.seeElement({ react: 'button' , props: { title : 'Požiadať o výmenu'}});
});

Scenario('[EDIT04] Verify, if lenght of subject will not be modified', async(I) => {
    I.click(locate('.block').withText('5IL210'));
    I.click({ react: 'button' , props: { title : 'Upraviť blok'}});

    I.clearField('length');
    I.fillField('length', '15');

    let length = I.grabValueFrom('length');
    I.assertEqual('9',(await length).toString());
    I.wait(1);
});