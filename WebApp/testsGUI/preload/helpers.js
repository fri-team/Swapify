require('./jquery');

window.getElementByXpath = function (path) {
    return document.evaluate(
        path,
        document,
        null,
        XPathResult.FIRST_ORDERED_NODE_TYPE,
        null,
    ).singleNodeValue;
};

window.parseRef = function (ref) {
    if (typeof ref === 'string') {
        return $(ref);
    }
    if (Array.isArray(ref)) {
        return ref.reduce((acc, el) => acc.add(el), $());
    }
    if (ref.length) {
        return ref;
    }
    if (
        typeof ref === 'object'
        && ref.locator
        && ref.type === 'xpath'
        && ref.locator.xpath
    ) {
        return $(getElementByXpath(ref.locator.xpath));
    }
    if (
        typeof ref === 'object'
        && ref.type === 'css'
        && ref.value
    ) {
        return $(ref.value);
    }
    return $(ref);
};

const oldOpen = XMLHttpRequest.prototype.open;
window.openHTTPs = 0;
window.openXHRs = [];
XMLHttpRequest.prototype.open = function (...args) {
    window.openHTTPs++;
    const self = this;
    window.openXHRs.push(this);
    this.addEventListener(
        'readystatechange',
        function () {
            if (this.readyState === 4) {
                window.openHTTPs--;
                window.openXHRs = window.openXHRs.filter(xhr => xhr !== self);
                if (typeof $ !== 'undefined') {
                    $(window).trigger('requests');
                }
            }
        },
        false,
    );
    return oldOpen.apply(this, args);
};

window.awaitRequests = includes => new Promise((resolve) => {
    if (window.openHTTPs === 0) {
        return resolve();
    }
    if (includes) {
        $(window).on(
            'requests',
            () => window.openXHRs.filter(item => item.__zone_symbol__xhrURL.includes(includes)).length === 0 && resolve(),
        );
    } else {
        $(window).on('requests', () => window.openHTTPs === 0 && resolve());
    }
});
