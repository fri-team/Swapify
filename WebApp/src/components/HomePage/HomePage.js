import React, { Component } from "react";
import { BrowserRouter as Router, Route, NavLink } from "react-router-dom";
import LoginPage from "../LoginPage/LoginPage";
import RegisterPage from "../RegisterPage/RegisterPage";
import "./HomePage.scss";
import AboutUsPage from "../AboutUsPage/AboutUsPage";
import { Typography, Box } from "@material-ui/core";

import Slider from "react-slick";
import homeImage from "../../images/home-background.png";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

import GlLogo from "../../images/GlobalLogic-logo-black.png";
import UnizaLogo from "../../images/Uniza-logo-black.png";

class HomePage extends Component {
  state = {
    isLoginPage: true,
  };

  changeState = () => {
    this.setState({
      isLoginPage: !this.state.isLoginPage,
    });
  };

  render() {
    const settings = {
      dots: true,
      arrows: false,
      infinite: true,
      autoplay: true,
      speed: 500,
      slidesToShow: 1,
      slidesToScroll: 1,
    };

    return (
      <Router>
        <div className="App">
          <div className="App__Aside">
            <div className="logo"></div>
            <div className="slider">
              <Slider {...settings}>
                <Box>
                  <img
                    className="slider_image"
                    src={homeImage}
                    height="60%"
                    width="60%"
                  ></img>
                </Box>
                <Box>
                  <div variant="body1" className="slider_text">
                    Sme študenti Fakulty riadenia a informatiky. Našou snahou je
                    vytvorenie <strong>jedinečnej aplikácie</strong> pre
                    študentov Žilinskej univerzity (najskôr však iba našej
                    fakulty), kde si budú môcť meniť svoj rozvrh a jednotlivé
                    predmety. Ideou je to, aby študenti nemuseli hľadať niekoho,
                    kto si taktiež bude chcieť{" "}
                    <strong>vymeniť predmet v rozvrhu</strong>, ale budú si môcť
                    predmety vymeniť jednoducho cez aplikáciu. Malo by to byť
                    miestom, kde si môžeš predmet zmeniť a systém za Teba nájde
                    osobu, ktorá chce opačnú výmenu.
                  </div>
                </Box>
                <Box>
                  <Typography styles={{ color: "red" }}>Treti text</Typography>
                </Box>
              </Slider>
            </div>
            <div className="App__Aside__footer">
              <img
                src={GlLogo}
                alt="Global Logic logo"
                height="30%"
                width="30%"
              />
              <img src={UnizaLogo} alt="Uniza logo" height="10%" width="10%" />
            </div>
          </div>
          <div className="App__Form">
            <Route exact path="/" component={LoginPage}></Route>
            <Route path="/register" component={RegisterPage}></Route>
            <Route path="/aboutus" component={AboutUsPage}></Route>
            {/* activeClassName="PageSwitcher__Item--Active" 
              className="PageSwitcher__Item"
            v oboch navlinkoch*/}
            <div className="PageSwitcher">
              {!this.state.isLoginPage && (
                <NavLink
                  exact
                  to="/"
                  onClick={this.changeState}
                  className="homePage_link"
                >
                  <div>Už máte účet?</div>
                  <div className="bold">&nbsp; Prihláste sa</div>
                </NavLink>
              )}

              {this.state.isLoginPage && (
                <NavLink
                  to="/register"
                  onClick={this.changeState}
                  className="homePage_link"
                >
                  <div>Nemáš účet? &nbsp;</div>
                  <div className="bold"> Zaregistruj sa</div>
                </NavLink>
              )}
            </div>
          </div>
        </div>
      </Router>
    );
  }
}

export default HomePage;
