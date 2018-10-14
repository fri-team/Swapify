import React from 'react';
import { Route, Switch } from 'react-router-dom';
import HomePage from './HomePage/HomePage';
import RegisterPage from './RegisterPage/RegisterPage';
import TimetablePage from './TimetablePage/TimetablePage';
import StudyGroup from './StudyGroup/StudyGroup';
import NotFoundPage from './NotFoundPage/NotFoundPage';

// This is a class-based component because the current version of hot reloading
// won't hot reload a stateless component at the top-level.

export default class App extends React.Component {
  render() {
    return (
      <div className="container">
        <Switch>
          <Route exact path="/" component={HomePage} />
          <Route path="/register" component={RegisterPage} />
          <Route path="/timetable" component={TimetablePage} />
          <Route path="/studyGroup" component={StudyGroup} />
          <Route component={NotFoundPage} />
        </Switch>
      </div>
    );
  }
}
