/* eslint-disable import/no-named-as-default */
import React from 'react';
import PropTypes from 'prop-types';
import SidebarContainer from '../containers/SidebarContainer/SidebarContainer';
import TimetableContainer from '../containers/TimetableContainer/TimetableContainer';

// This is a class-based component because the current
// version of hot reloading won't hot reload a stateless
// component at the top-level.

class App extends React.Component {
  render() {
    return (
      <div className="container">
        <SidebarContainer />
        <TimetableContainer />
      </div>
    );
  }
}

App.propTypes = {
  children: PropTypes.element,
};

export default App;
