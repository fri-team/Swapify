import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import React, { Component } from "react";
import Notification from './Notification';
import './NotificationPanel.scss';
import { fetchNotifications, setRead } from '../../actions/notificationsActions';

class NotificationPanel extends Component {
    state = {
        notificationPanelVisibility: ''
    }

    componentDidMount() {
        this.props.fetchNotifications();
    }

    onProfileClick = () => {
        this.setState({
            notificationPanelVisibility: this.state.notificationPanelVisibility ? '' : 'visible'
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
                        key={notification.notificationId}
                        id={notification.notificationId}
                        text={notification.text}
                        read={notification.read}
                        //date={moment(notification.createdAt).format("ddd, Do MMM YYYY - HH:mm")}
                        setNotificationRead = {this.props.setNotificationRead}
                    />
                );                                
            });
        } else {
            notificationComponents = <div className="empty-notifications">There are currently no notifications</div>;            
        }

        let unreadNotificationsCount = this.countUnreadNotifications(this.props.notifications);
        var badge = '';

        if(unreadNotificationsCount > 0) {
            badge = 'visible';
        } else {
            badge = '';
        }        

        return (
            <div className="notification-wrapper">                                  
                <div className="notification-icon" onClick={this.onProfileClick}></div>                
                <div className={"notification-badge" + badge} onClick={this.onProfileClick}>{unreadNotificationsCount}</div>
                <div className={"notification-panel " + this.state.notificationPanelVisibility}>
                    <div className="notification-panel-header clearfix">
                        <strong>Notifications</strong>
                        <p>Mark All as Read</p>
                    </div>
                    <div className="notification-panel-content">
                        {notificationComponents}
                    </div>
                    <div className="notification-panel-footer clearfix">
                        <strong>Show all notifications</strong>
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

export default connect(mapStateToProps, mapDispatchToProps)(NotificationPanel);