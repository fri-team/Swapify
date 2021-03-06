import React, { PureComponent } from 'react';
import styled from 'styled-components';
import { connect } from 'react-redux';
import AppBar from '@material-ui/core/AppBar';
import Toolbar from '@material-ui/core/Toolbar';
import IconButton from '@material-ui/core/IconButton';
import MenuIcon from '@material-ui/icons/Menu';
import HelpIcon from '@material-ui/icons/Help';
import UserAvatar from './UserAvatar';
import Menu from './Menu';
import { PullRight } from './shared';
import { Button } from '@material-ui/core';
import { bindActionCreators } from 'redux';
import * as userActions from '../../actions/userActions';
import * as timetableActions from '../../actions/timetableActions';
import { withRouter } from 'react-router-dom';
import { PERSONALNUMBER, TIMETABLE } from '../../util/routes';
import NotificationPanel from '../Notifications/NotificationPanel';
import Tooltip from '@material-ui/core/Tooltip';
import Zoom from '@material-ui/core/Zoom';
import './Toolbar.scss';
import MailIcon from '@material-ui/icons/Mail';
import logo from '../../images/logowhite.png';

const ToolbarWrapper = styled.div`
  width: 100%;
  z-index: 5;
`;

const IconTray = styled.div`
  width: 100%;
  display: flex;
`;

class AppToolbar extends PureComponent {
  state = { showMenu: false, changeGroup: false };

  handleLogout = () => this.props.userActions.logout();

  changePersonalNumber = () => this.props.history.push(PERSONALNUMBER);

  timetable = () => this.props.history.push(TIMETABLE);

  checkUrl = () => {
    if(window.location.pathname == "/personal-number") {
      return true;
    } else {
      return false;
    }
  }
  
  render() {
    
    let button;
    if (this.props.timetable.isExchangeMode) {
      button = (
        <Button
          variant="contained"
          color="default"
          size="small"
          className="backToTimetable"
          onClick={() => {
            this.props.timetableActions.cancelExchangeMode();
            this.props.timetableActions.hideCourseTimetable();
          }}
        >
          Späť na rozvrh
        </Button>
      );
    }

    const { user, toggleSidebar, toggleHelpModalWindow, toggleMailUsModalWindow } = this.props;
    const url = this.checkUrl();
    return (
      <ToolbarWrapper>
        <AppBar position="static">
          <Toolbar>
            { !url && (
            <IconButton
              color="inherit"
              aria-label="Menu"
              onClick={toggleSidebar}
            >
              <MenuIcon />
            </IconButton>
            )}
           
            <IconTray>
            
            <img src={logo} alt="logo" height="30px" className="logowhite" onClick={this.timetable}/>
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
                  selectPersonalNumber={this.changePersonalNumber}
                  onLogout={this.handleLogout}
                  onClose={() => this.setState({ showMenu: false })}
                />
              )}
            </IconTray>
            &nbsp;
            <p onClick={() => this.setState({ showMenu: true })} className="cursor">
              {user.name} {user.surname}
            </p>
            
            <Tooltip title="Zobraz pomocník" placement="top" TransitionComponent={Zoom}>
              <IconButton
                color="inherit"
                aria-label="Help"
                onClick={toggleHelpModalWindow}
              >
                <HelpIcon />
              </IconButton>
            </Tooltip>
            
            <Tooltip title="Napíšte nám" placement="top" TransitionComponent={Zoom}>
              <IconButton
                color="inherit"
                aria-label="MailUs"
                onClick={toggleMailUsModalWindow}
              >
                <MailIcon />
              </IconButton>
            </Tooltip>
            
            <Tooltip title="Notifikácie" placement="top" TransitionComponent={Zoom}>
              <IconButton
                color="inherit"
                aria-label="Notifications"
              >
                <NotificationPanel />
              </IconButton>
            </Tooltip>
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
