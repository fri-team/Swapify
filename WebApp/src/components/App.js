import React, { Component } from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import { onlyAuth, onlyNotAuth } from '../util/auth';
import { HOME, LOGIN, REGISTER, CONFIRMEMAIL, TIMETABLE, SETNEWPASSWORD, PERSONALNUMBER } from '../util/routes';

import {
  HomePage,
  RegisterPage,
  ConfirmEmailPage,
  LoginPage,
  TimetablePage,
  NotFoundPage,
  SetNewPasswordPage,
  PersonalNumber
} from './';

// This is a class-based component because the current version of hot reloading
// won't hot reload a stateless component at the top-level.

export default class App extends Component {
  render() {
    return (
      <BrowserRouter>
        <div className="container">
          <Switch>
            <Route exact path={HOME} component={onlyNotAuth(HomePage)} />
            <Route path={LOGIN} component={onlyNotAuth(LoginPage)} />
            <Route path={PERSONALNUMBER} component={onlyAuth(PersonalNumber)} />
            <Route path={REGISTER} component={onlyNotAuth(RegisterPage)} />
            <Route path={CONFIRMEMAIL} component={onlyNotAuth(ConfirmEmailPage)} />
            <Route path={TIMETABLE} component={onlyAuth(TimetablePage)} />
            <Route path={SETNEWPASSWORD} component={onlyNotAuth(SetNewPasswordPage)} />            
            <Route component={NotFoundPage} />
          </Switch>
        </div>
      </BrowserRouter>
    );
  }
}
