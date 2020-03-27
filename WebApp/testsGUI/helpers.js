module.exports = {
    catchWithScreenshot(fn) {
        return I => fn(I).catch(err => console.error(err));
    },
    compareStrings(str1, str2) {
        return (str1 || '').toString().trim() === (str2 || '').toString().trim();
    },
};
