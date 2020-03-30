/* eslint-disable no-undef */
/* eslint-disable class-methods-use-this */
class Puppeteer extends Helper {
    _init() { }

    async realMouseover(selector) {
        const { browser } = this.helpers.Puppeteer;
    }

    async getPuppeteer() {
        return this.helpers.Puppeteer;
    }

    async makeRequest(method, url, data) {
        return this.exec(
            options => new Promise((resolve, reject) => $.ajax(
                Object.assign(options, {
                    complete(xhr) {
                        const message = xhr.responseJSON || xhr.responseText;
                        if (xhr.status !== 200) {
                            return reject(
                                new Error(
                                    `Got status ${xhr.status} with message ${message}`,
                                ),
                            );
                        }
                        return resolve(message);
                    },
                }),
            )),
            {
                method,
                url,
                data,
            },
        );
    }

    async exec(fn, ...args) {
        if (!fn || typeof fn !== 'function') {
            return null;
        }
        const { browser } = this.helpers.Puppeteer;
        return browser.evaluate(
            (fnString, fnArgs, done) => {
                const fn = eval(fnString);
                try {
                    const res = fn.apply(null, fnArgs);
                    if (res && res.then) {
                        return res
                            .then(result => done(null, result))
                            .catch(err => done(err, null));
                    }
                    return done(null, res);
                } catch (err) {
                    return done(err, null);
                }
            },
            fn.toString(),
            args,
        );
    }

    async realMouseOver(selector) {
        return this.exec(async (selector) => {
            const $el = parseRef(selector);
            if (!$el) {
                throw new Error('Element was not found');
            }
            const { x, y } = $el.offset();
            window.webContents.sendInputEvent({
                type: 'mouseEnter',
                x,
                y,
            });
        }, selector);
    }

    async step(message) {
        console.log(`     ${message}`.green);
    }

    // add custom methods here
    // If you need to access other helpers
    // use: this.helpers['helperName']
}

module.exports = Puppeteer;
