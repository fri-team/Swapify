import React from 'react';
import PropTypes from 'prop-types';
import Toolbar from '../Toolbar/Toolbar';
import SidebarContainer from '../../containers/SidebarContainer/SidebarContainer';
import TimetableContainer from '../../containers/TimetableContainer/TimetableContainer';
import BlockDetailContainer from '../../containers/BlockDetailContainer/BlockDetailContainer';

class TimetablePage extends React.Component {
  state = {
    sidebarOpen: true,
  }

  toggleSidebar = () => {
    this.setState({ sidebarOpen: !this.state.sidebarOpen });
  }

  render() {
    return (
      <div className="container">
        <Toolbar toggleSidebar={this.toggleSidebar} />
        <div className="sidebar-timetable">
          <SidebarContainer open={this.state.sidebarOpen} />
          <TimetableContainer />
        </div>
        <BlockDetailContainer />
      </div>
    );
  }
}

TimetablePage.propTypes = {
  children: PropTypes.element,
};

export default TimetablePage;
