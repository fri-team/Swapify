import React, { PureComponent } from 'react';
import styled from 'styled-components';
import { connect } from 'react-redux';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import IconButton from '@material-ui/core/IconButton';
import MenuIcon from '@material-ui/icons/Menu';
import { logout } from '../../actions/userActions';
import UserAvatar from './UserAvatar';
import Menu from './Menu';
import { PullRight } from './shared';
import { withRouter } from 'react-router-dom';
import {STUDYGROUP} from '../../util/routes'

const ToolbarWrapper = styled.div`
  width: 100%;
`;

const IconTray = styled.div`
  width: 100%;
  display: flex;
`;

class AppToolbar extends PureComponent {
  state = { showMenu: false };

  handleLogout = () => this.props.dispatch(logout());

  changeGroup = () => this.props.history.push(STUDYGROUP);

  render() {
    const { user, toggleSidebar } = this.props;
    return (
      <ToolbarWrapper>
        <AppBar position="static">`
          <Toolbar>
            <IconButton
              color="inherit"
              aria-label="Menu"
              onClick={toggleSidebar}
            >
              <MenuIcon />
            </IconButton>
            <IconTray>
              <PullRight />
              <UserAvatar
                ref={ref => (this.anchor = ref)}
                username={user.name}
                onClick={() => this.setState({ showMenu: true })}
              />
              {this.state.showMenu && (
                <Menu
                  renderRef={this.anchor}
                  username={`${user.name} ${user.surname}`}
                  email={user.email}
                  selectStudyGroup={this.changeGroup}
                  onLogout={this.handleLogout}
                  onClose={() => this.setState({ showMenu: false })}
                />
              )}
            </IconTray>
          </Toolbar>
        </AppBar>
      </ToolbarWrapper>
    );
  }
}

const mapStateToProps = state => ({ user: state.user });

export default connect(mapStateToProps)(withRouter(AppToolbar));
