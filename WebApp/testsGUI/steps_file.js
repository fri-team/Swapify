/* eslint-disable no-undef */

// in this file you can append custom step methods to 'I' object
module.exports = function () {
    return actor({
        // Define custom steps here, use 'this' to access default methods of I.
        // It is recommended to place a general 'login' function here.

        async login() {
            const I = this;
            I.amOnPage('/');
            I.fillField('input[type="email"]', 'Oleg@swapify.com');
            I.fillField('input[type="password"]', 'Heslo123');
            I.click(locate('button').withText('Prihlásiť sa'));
        },

        async changePersonalNumber(persNum) {
            const I = this;
            I.click(locate('.cursor'));
            I.click('Zmeniť číslo');
            I.see('Zadajte osobné číslo');
            I.seeElement({ react: 'TextField' , props: { id : 'personalNumber' }});
            I.fillField('Zadajte osobné číslo', persNum);
            I.click('Uložiť');
        },
        
        async updateField(fieldName, value) {
            const I = this;
            I.click(fieldName);
            I.pressKey(['Shift', 'Home']);
            I.pressKey(value);
        },

        async eval(fn, message, ...args) {
            message && this.say(message);
            const { browser } = await this.getNightmare();
            return browser.evaluate(fn, ...args);
        },
        async awaitRequests(includes) {
            const I = this;
            I.executeAsyncScript((includes, done) => {
                awaitRequests(includes).then(done);
            }, includes);
            I.wait(1);
        },

        async removeNextLinesFromString(str) {
            const ret = str;
            ret.replace(/\n/g, ' ');
            return ret;
        },

        async ctrlDown() {
            await this.exec(() => document.dispatchEvent(new KeyboardEvent('keydown', { ctrlKey: true })));
        },

        async ctrlUp() {
            await this.exec(() => document.dispatchEvent(new KeyboardEvent('keyup', { ctrlKey: false })));
        },

        // click on element
        async mouseEnter(locator) {
            await this.exec((loc) => {
                parseRef(loc)[0].dispatchEvent(new MouseEvent('mouseenter'));
            }, locator);
        },

        async addBlock(row, column) {
            const I = this;
            I.click({ react: 'Block', props: { i: row, j: column }});
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
        },

        async deleteBlock(row, column) {
            const I = this;
            I.click({ react: 'Block', props: { i: row, j: column }});
            I.click({ react: 'button' , props: { title : 'Vymazať blok'}});
        }

    });
};
