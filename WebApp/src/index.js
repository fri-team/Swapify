/* eslint-disable import/default */
import "babel-polyfill";
import React from "react";
import { render } from "react-dom";
import { AppContainer } from "react-hot-loader";

// redux
import { Provider as ReduxProvider } from "react-redux";
import { store as storeNew, persistor as persistorNew } from "./redux/store";
import { PersistGate } from "redux-persist/lib/integration/react";

import configureStore, { history } from "./store/configureStore";
import Root from "./components/Root";
import "./styles/styles.scss";
require("./favicon.ico");

const { store, persistor } = configureStore();

render(
  <AppContainer>
    <ReduxProvider store={storeNew}>
      <PersistGate loading={null} persistor={persistorNew}>
        <Root store={store} persistor={persistor} history={history} />
      </PersistGate>
    </ReduxProvider>
  </AppContainer>,
  document.getElementById("app")
);

if (module.hot) {
  module.hot.accept("./components/Root", () => {
    const NewRoot = require("./components/Root").default;
    render(
      <AppContainer>
        <ReduxProvider store={storeNew}>
          <PersistGate loading={null} persistor={persistorNew}>
            <NewRoot store={store} persistor={persistor} history={history} />
          </PersistGate>
        </ReduxProvider>
      </AppContainer>,
      document.getElementById("app")
    );
  });
}
