import React, { PureComponent } from 'react';
import Toolbar from '../Toolbar/Toolbar';
import { connect } from 'react-redux';
import TimetableContainer from '../../containers/TimetableContainer/TimetableContainer';
import BlockDetailContainer from '../../containers/BlockDetailContainer/BlockDetailContainer';
import SidebarContainer from '../Sidebar/SidebarContainer';

import './TimetablePage.scss';

class TimetablePage extends PureComponent {
  state = { sidebarOpen: false, user: this.props.user };
  
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
        <BlockDetailContainer 
          user={this.state.user}
         />
      </div>
    );
  }
}


const mapStateToProps = state => ({ user: state.user });

export default connect(mapStateToProps)(TimetablePage);
