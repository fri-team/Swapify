import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { ThemeProvider } from 'styled-components';
import { Provider } from 'react-redux';
import { PersistGate } from 'redux-persist/integration/react';
import { ConnectedRouter } from 'connected-react-router'
import { MuiThemeProvider, createMuiTheme } from '@material-ui/core/styles';
import { blueGrey, blue, grey } from '@material-ui/core/colors';
import { App, LoadingPage } from './';

const styledTheme = {
  color: {
    primary: blue[500],
    secondary: grey[500],
    tetriary: blueGrey[500]
  }
};

const muiTheme = createMuiTheme({
  typography: {
    useNextVariants: true
  },
  palette: {
    primary: blue,
    secondary: grey,
    tetriary: blueGrey
  }
});

export default class Root extends Component {
  render() {
    const { store, persistor, history } = this.props;
    return (
        <Provider store={store}>
          <PersistGate persistor={persistor} loading={LoadingPage()}>
            <ConnectedRouter history={history}>
              <ThemeProvider theme={styledTheme}>
                <MuiThemeProvider theme={muiTheme}>
                  <App />
                </MuiThemeProvider>
              </ThemeProvider>
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
