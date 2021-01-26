import React, { Component } from 'react';
import { BrowserRouter as Router, Route, NavLink } from 'react-router-dom';
import LoginPage from '../LoginPage/LoginPage';
import RegisterPage from '../RegisterPage/RegisterPage';
import './HomePage.scss';
import { AboutUsPage } from '..';

class HomePage extends Component {

  render() {
    return (
      <Router>
        <div className="App">
          <div className="App__Aside">
            <div className="logo"></div>
            <div className="home-image"></div>
          </div>
          <div className="App__Form">
            <div className="PageSwitcher">
              <NavLink exact to="/" activeClassName="PageSwitcher__Item--Active" className="PageSwitcher__Item">Prihlásenie</NavLink>
              <NavLink to="/register" activeClassName="PageSwitcher__Item--Active" className="PageSwitcher__Item">Registrácia</NavLink>
            </div>
            <Route exact path="/" component={LoginPage}>
            </Route>
            <Route path="/register" component={RegisterPage}>
            </Route>
            <Route path="/aboutus" component={AboutUsPage}>
            </Route>
            <div className="HomeFooter">
              <NavLink exact to="/aboutus" className="FormField__Link">O nás</NavLink>
            </div>
          </div>
        </div>
      </Router>
    );
  }
}

export default (HomePage);
