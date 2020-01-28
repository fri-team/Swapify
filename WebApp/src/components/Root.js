import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { ThemeProvider } from 'styled-components';
import { Provider } from 'react-redux';
import { PersistGate } from 'redux-persist/integration/react';
import { ConnectedRouter } from 'connected-react-router'
import { MuiThemeProvider, createMuiTheme } from '@material-ui/core/styles';
import blue from '@material-ui/core/colors/blue';
import pink from '@material-ui/core/colors/pink';
import { App, LoadingPage } from './';

const styledTheme = {
  color: {
    primary: blue[500],
    secondary: pink[500]
  }
};

const muiTheme = createMuiTheme({
  typography: {
    useNextVariants: true
  },
  palette: {
    primary: blue,
    secondary: pink
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
