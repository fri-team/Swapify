

import React, { Component } from 'react';
import { Route, NavLink } from 'react-router-dom';
import LoginPage from '../LoginPage/LoginPage';
import ForgotPasswordPage from '../ForgotPasswordPage/ForgotPasswordPage';
import './HomePage.scss';

class HomePage extends Component {
  render() {
    return (
        <div className="App">
          <div className="App__Aside"></div>
          <div className="App__Form">
            <div className="PageSwitcher">
                <NavLink to="/" activeClassName="PageSwitcher__Item--Active" className="PageSwitcher__Item">Prihlásiť sa</NavLink>
                <NavLink exact to="/register" activeClassName="PageSwitcher__Item--Active" className="PageSwitcher__Item">Registrovať sa</NavLink>
              </div>

              <Route exact path="/" component={LoginPage}>
              </Route>
              <Route path="/register" component={LoginPage}>
              </Route>
              <Route path="/forgot-password" component={ForgotPasswordPage}>
              </Route>
          </div>

        </div>
    );
  }
}

export default (HomePage);