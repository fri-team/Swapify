import React, { PureComponent } from 'react';
import Toolbar from '../Toolbar/Toolbar';
import TimetableContainer from '../../containers/TimetableContainer/TimetableContainer';
import BlockDetailContainer from '../../containers/BlockDetailContainer/BlockDetailContainer';
import SidebarContainer from '../Sidebar/SidebarContainer';

export default class TimetablePage extends PureComponent {
  state = { sidebarOpen: false };

  render() {
    return (
      <div className="container">
        <Toolbar
          toggleSidebar={() =>
            this.setState(prevState => ({
              sidebarOpen: !prevState.sidebarOpen
            }))
          }
        />
        <SidebarContainer
          open={this.state.sidebarOpen}
          onClose={() => this.setState({ sidebarOpen: false })}
        />
        <TimetableContainer />
        <BlockDetailContainer />
      </div>
    );
  }
}
