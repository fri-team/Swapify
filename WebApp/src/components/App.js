import React from 'react';
import { Route, Switch } from 'react-router-dom';
<<<<<<< HEAD
import HomePage from './HomePage/HomePage';
import RegisterPage from './RegisterPage/RegisterPage';
import TimetablePage from './TimetablePage/TimetablePage';
import StudyGroup from './StudyGroup/StudyGroup';
import NotFoundPage from './NotFoundPage/NotFoundPage';
=======
import { onlyAuth, onlyNotAuth } from '../util/auth';
import {
  HomePage,
  RegisterPage,
  LoginPage,
  TimetablePage,
  NotFoundPage
} from './';
>>>>>>> origin/develop

// This is a class-based component because the current version of hot reloading
// won't hot reload a stateless component at the top-level.

export default class App extends React.Component {
  render() {
    return (
      <div className="container">
        <Switch>
<<<<<<< HEAD
          <Route exact path="/" component={HomePage} />
          <Route path="/register" component={RegisterPage} />
          <Route path="/timetable" component={TimetablePage} />
          <Route path="/studyGroup" component={StudyGroup} />
=======
          <Route exact path="/" component={onlyNotAuth(HomePage)} />
          <Route path="/login" component={onlyNotAuth(LoginPage)} />
          <Route path="/register" component={onlyNotAuth(RegisterPage)} />
          <Route path="/timetable" component={onlyAuth(TimetablePage)} />
>>>>>>> origin/develop
          <Route component={NotFoundPage} />
        </Switch>
      </div>
    );
  }
}
