import React from 'react';
import { Route, Switch } from 'react-router-dom';
import { onlyAuth, onlyNotAuth } from '../util/auth';
import {
  HomePage,
  RegisterPage,
  LoginPage,
  TimetablePage,
  NotFoundPage
} from './';

// This is a class-based component because the current version of hot reloading
// won't hot reload a stateless component at the top-level.

export default class App extends React.Component {
  render() {
    return (
      <div className="container">
        <Switch>
          <Route exact path="/" component={onlyNotAuth(HomePage)} />
          <Route path="/login" component={onlyNotAuth(LoginPage)} />
          <Route path="/register" component={onlyNotAuth(RegisterPage)} />
          <Route path="/timetable" component={onlyAuth(TimetablePage)} />
          <Route component={NotFoundPage} />
        </Switch>
      </div>
    );
  }
}
