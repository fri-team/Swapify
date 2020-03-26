exports.config = {
    tests: './**/*.spec.js',
    output: './output',
    helpers: {
        Nightmare: {
            url: 'http://localhost:3000',
            show: true,
            restart: true,
            webPreferences: {
                preload: require.resolve('./preload'),
            },
            desiredCapabilities: {
                browserName: 'chrome',
            },
            openDevTools: true,
            pollInterval: 5000,
            height: 900,
            width: 1890,
            waitForAction: 40, // default delay after each command
        },
        NightmareWrap: {
            require: './nightmare_helper.js',
        },
        AssertWrapper: {
            require: './node_modules/codeceptjs-assert',
        },
    },
    plugins: {
        autoDelay: {
            enabled: true,
            delayAfter: 200, // default delay
            methods: ['click', 'say', 'see', 'awaitRequests'],
        },
    },
    include: {
        I: './steps_file.js',
    },
    bootstrap: null,
    mocha: {},
    name: 'testsGUI',
};
