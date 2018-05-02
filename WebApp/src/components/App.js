import React from 'react';
import PropTypes from 'prop-types';
import { Route, Switch } from 'react-router-dom';
import HomePage from './HomePage/HomePage';
import TimetablePage from './TimetablePage/TimetablePage';
import NotFoundPage from './NotFoundPage/NotFoundPage';

// This is a class-based component because the current version of hot reloading
// won't hot reload a stateless component at the top-level.

class App extends React.Component {
  render() {
    return (
      <div>
        <Switch>
          <Route exact path="/" component={HomePage} />
          <Route path="/timetable" component={TimetablePage} />
          <Route component={NotFoundPage} />
        </Switch>
      </div>
    );
  }
}

App.propTypes = {
  children: PropTypes.element,
};

export default App;
