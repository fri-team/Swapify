

import React, { Component } from 'react';
import { BrowserRouter as Router, Route, NavLink } from 'react-router-dom';
import LoginPage from '../LoginPage/LoginPage';
import RegisterPage from '../RegisterPage/RegisterPage';
import { Fade } from 'react-slideshow-image';
import './HomePage.scss';




class HomePage extends Component {

  render() {
    const properties = {
      duration: 5000,
      transitionDuration: 500,
      infinite: true,
      indicators: true,
      arrows: true
    }
    return (
      <Router>
        <div className="App">
          <div className="App__Aside">
            <div className="logo"></div>
            <div className="slide-container">
              <Fade {...properties}>
                <div className="each-slide">
                  <div>
                    <span>Nesedí ti rozvrh? Nevadí! SWAPIFY ti pomôže</span>
                  </div>
                </div>
                <div className="each-slide">
                  <div >
                    <span>Vymeniť si „cvičenia“ nikdy nebolo ľahšie</span>
                  </div>
                </div>
                <div className="each-slide">
                  <div>
                    <span>SWAPIFY ti ponúka prehľadnosť, komplexnosť a jednoduchosť</span>
                  </div>
                </div>
              </Fade>
            </div>
          </div>
          <div className="App__Form">
            <div className="PageSwitcher">
              <NavLink exact to="/" activeClassName="PageSwitcher__Item--Active" className="PageSwitcher__Item">Prihlásiť sa</NavLink>
              <NavLink to="/register" activeClassName="PageSwitcher__Item--Active" className="PageSwitcher__Item">Registrovať sa</NavLink>
            </div>
            <Route exact path="/" component={LoginPage}>
            </Route>
            <Route path="/register" component={RegisterPage}>
            </Route>
          </div>

        </div>
      </Router>
    );
  }
}

export default (HomePage);