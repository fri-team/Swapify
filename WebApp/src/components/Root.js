import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { Provider } from 'react-redux';
import { PersistGate } from 'redux-persist/integration/react';
import { ConnectedRouter } from 'react-router-redux';
import { MuiThemeProvider, createMuiTheme } from '@material-ui/core/styles';
import blue from '@material-ui/core/colors/blue';
import { App, LoadingPage } from './';

const theme = createMuiTheme({
  palette: {
    primary: blue
  }
});

export default class Root extends Component {
  render() {
    const { store, persistor, history } = this.props;
    return (
      <Provider store={store}>
        <PersistGate persistor={persistor} loading={<LoadingPage />}>
          <ConnectedRouter history={history}>
            <MuiThemeProvider theme={theme}>
              <App />
            </MuiThemeProvider>
          </ConnectedRouter>
        </PersistGate>
      </Provider>
    );
  }
}

Root.propTypes = {
  store: PropTypes.object.isRequired,
  history: PropTypes.object.isRequired
};
