exports.config = {
  tests: "./scenarios/*.spec.js",
  output: "./output",
  helpers: {
    Puppeteer: {
      url: "http://localhost:3000",
      show: true,
      restart: true,
      webPreferences: {
        preload: require.resolve("./preload")
      },
      desiredCapabilities: {
        browserName: "chrome"
      },
      openDevTools: true,
      pollInterval: 5000,
      height: 900,
      width: 1890,
      waitForAction: 40 // default delay after each command
    },
    PuppeteerWrap: {
      require: "./puppeteer_helper.js"
    },
    AssertWrapper: {
      require: "./node_modules/codeceptjs-assert"
    }
  },
  plugins: {
    autoDelay: {
      enabled: true,
      delayAfter: 200, // default delay
      methods: ["click", "say", "see", "awaitRequests"]
    },

    retryFailedStep: {
      enabled: true
    },
    screenshotOnFail: {
      enabled: true
    }
  },
  include: {
    I: "./steps_file.js"
  },
  bootstrap: null,
  mocha: {},
  name: "testsGUI"
};
