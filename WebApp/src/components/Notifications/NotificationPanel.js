import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import React, { Component } from "react";
import Notification from './Notification';
import onClickOutside from 'react-onclickoutside';
import NotificationIcon from '../svg/NotificationIcon';
import { fetchNotifications, setRead } from '../../actions/notificationsActions';
import './NotificationPanel.scss';
import { NOTIFICATIONS_FETCH_INTERVAL_SECONDS } from '../../constants/configurationConstants';

class NotificationPanel extends Component {
    state = {
        notificationPanelVisibility: ''
    }

    componentDidMount() {
        this.props.fetchNotifications();
        this.notificationsFetchInterval = setInterval(() => this.props.fetchNotifications(),
            NOTIFICATIONS_FETCH_INTERVAL_SECONDS * 1000);        
    }

    componentWillUnmount() {
        clearInterval(this.notificationsFetchInterval);
    }

    onProfileClick = () => {
        this.setState({
            notificationPanelVisibility: this.state.notificationPanelVisibility ? '' : 'visible'
        });
    }    

    handleClickOutside = () => {
        this.setState({
            notificationPanelVisibility: ''
        });
      }

    countUnreadNotifications(notifications) {
        var unreadNotificationsCount = 0;
    
        if(Array.isArray(notifications) && notifications.length > 0) {
            notifications.forEach(function(notification) {
                if(notification.read == false) {
                    unreadNotificationsCount++;
                }
            });
        }
        
        return unreadNotificationsCount;
    }

    render() { 
        let notificationComponents = [];

        if (this.props.notifications && Array.isArray(this.props.notifications) && this.props.notifications.length > 0) {
            this.props.notifications.forEach(notification => {
                notificationComponents.push(
                    <Notification
                        key={notification.id}
                        id={notification.id}
                        text={notification.text}
                        read={notification.read}                        
                        setNotificationRead = {this.props.setNotificationRead}
                    />
                );                                
            });
        } else {
            notificationComponents = <div className="empty-notifications">Momentálne nemáte žiadne notifikácie.</div>;            
        }

        let unreadNotificationsCount = this.countUnreadNotifications(this.props.notifications);
        var badge = '';

        if(unreadNotificationsCount > 0) {
            badge = 'visible';
        } else {
            badge = '';
        }        

        return (
            <div className="">                                                                  
                <div className={"notification-badge" + badge} onClick={this.onProfileClick}>{unreadNotificationsCount}</div>
                <NotificationIcon onClick={this.onProfileClick}/>
                <div className={"notification-panel " + this.state.notificationPanelVisibility}>
                    <div className="notification-panel-header clearfix">
                        <strong>Notifikácie</strong>                        
                    </div>
                    <div className="notification-panel-content">
                        {notificationComponents}
                    </div>                    
                </div>
            </div>
        );
    }
}

NotificationPanel.propTypes = {
    notifications: PropTypes.array,
    unreadNotificationsCount: PropTypes.number
}

const mapStateToProps = state => ({ ...state.notifications });
const mapDispatchToProps = dispatch => {
  return {
      fetchNotifications: () => dispatch(fetchNotifications()),
      setNotificationRead: (notificationId, read) => dispatch(setRead(notificationId, read))
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(onClickOutside(NotificationPanel));
