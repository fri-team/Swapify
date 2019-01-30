import React, { PureComponent } from 'react';
import styled from 'styled-components';
import { connect } from 'react-redux';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import IconButton from '@material-ui/core/IconButton';
import MenuIcon from '@material-ui/icons/Menu';
import UserAvatar from './UserAvatar';
import Menu from './Menu';
import { PullRight } from './shared';
import { Button } from '@material-ui/core';
import { bindActionCreators } from 'redux';
import * as userActions from '../../actions/userActions';
import * as timetableActions from '../../actions/timetableActions';
import { withRouter } from 'react-router-dom';
import { STUDYGROUP } from '../../util/routes';

const ToolbarWrapper = styled.div`
  width: 100%;
`;

const IconTray = styled.div`
  width: 100%;
  display: flex;
`;

class AppToolbar extends PureComponent {
  state = { showMenu: false };

  handleLogout = () => this.props.userActions.logout();

  changeGroup = () => this.props.history.push(STUDYGROUP);

  render() {
    let button;
    if (this.props.timetable.isExchangeMode) {
      button = (
        <Button
          variant="contained"
          color="default"
          onClick={() => this.props.timetableActions.cancelExchangeMode()}
        >
          Späť na rozvrh
        </Button>
      );
    }

    const { user, toggleSidebar } = this.props;
    return (
      <ToolbarWrapper>
        <AppBar position="static">
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
              {button}
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

const mapStateToProps = state => ({
  user: state.user,
  timetable: state.timetable
});

const mapDispatchToProps = dispatch => ({
  userActions: bindActionCreators(userActions, dispatch),
  timetableActions: bindActionCreators(timetableActions, dispatch)
});

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(withRouter(AppToolbar));
