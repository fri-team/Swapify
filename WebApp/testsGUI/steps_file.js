// in this file you can append custom step methods to 'I' object
module.exports = function () {
    return actor({
        // Define custom steps here, use 'this' to access default methods of I.
        // It is recommended to place a general 'login' function here.
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

    });
};
