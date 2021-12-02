/* eslint-disable import/default */
import 'babel-polyfill';
import React from 'react';
import { render } from 'react-dom';
import { AppContainer } from 'react-hot-loader';
import configureStore, { history } from './store/configureStore';
import Root from './components/Root';
import './styles/styles.scss';
require('./favicon.ico');

const { store, persistor } = configureStore();

render(
  <AppContainer>
    <Root store={store} persistor={persistor} history={history} />
  </AppContainer>,
  document.getElementById('app')
);

if (module.hot) {
  module.hot.accept('./components/Root', () => {
    const NewRoot = require('./components/Root').default;
    render(
      <AppContainer>
        <NewRoot store={store} persistor={persistor} history={history} />
      </AppContainer>,
      document.getElementById('app')
    );
  });
}
