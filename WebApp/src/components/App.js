import React, { Component } from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import { onlyAuth, onlyNotAuth } from '../util/auth';
import { HOME, LOGIN, REGISTER, TIMETABLE } from '../util/routes';
import {
  HomePage,
  RegisterPage,
  LoginPage,
  TimetablePage,
  NotFoundPage
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
            <Route path={REGISTER} component={onlyNotAuth(RegisterPage)} />
            <Route path={TIMETABLE} component={onlyAuth(TimetablePage)} />
            <Route component={NotFoundPage} />
          </Switch>
        </div>
      </BrowserRouter>
    );
  }
}
