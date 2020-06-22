import React, { Component } from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import { onlyAuth, onlyNotAuth } from '../util/auth';
import { HOME, LOGIN, REGISTER, CONFIRMEMAIL, TIMETABLE, SETNEWPASSWORD, PERSONALNUMBER, ABOUTUS, PRIVACYPOLICY } from '../util/routes';

import {
  HomePage,
  RegisterPage,
  ConfirmEmailPage,
  //LoginPage,
  AboutUsPage,
  TimetablePage,
  NotFoundPage,
  SetNewPasswordPage,
  PersonalNumber
} from './';
import PrivacyPolicyPage from './PrivacyPolicyPage/PrivacyPolicyPage';
//import WebViewer from './PrivacyPolicyPage/web/viewer';

// This is a class-based component because the current version of hot reloading
// won't hot reload a stateless component at the top-level.

export default class App extends Component {
  render() {
    return (
      <BrowserRouter>
        <div className="app-container">
          <Switch>
            <Route exact path={HOME} component={onlyNotAuth(HomePage)} />
            <Route path={LOGIN} component={onlyNotAuth(HomePage)} />
            <Route path={PERSONALNUMBER} component={onlyAuth(PersonalNumber)} />
            <Route path={REGISTER} component={onlyNotAuth(RegisterPage)} />
            <Route path={ABOUTUS} component={onlyNotAuth(AboutUsPage)} />
            <Route path={TIMETABLE} component={onlyAuth(TimetablePage)} />
            <Route path={SETNEWPASSWORD} component={onlyNotAuth(SetNewPasswordPage)} />
            <Route path={PRIVACYPOLICY} component={onlyNotAuth(PrivacyPolicyPage)} /> 
            <Route path={CONFIRMEMAIL} component={onlyNotAuth(ConfirmEmailPage)} />
            <Route component={NotFoundPage} />
          </Switch>
          
        </div>
      </BrowserRouter>
    );
  }
}
