import { connect } from 'react-redux';
import PropTypes from 'prop-types';
import React, { Component } from "react";
import Notification from './Notification';
import './NotificationPanel.scss';
import { fetchNotifications } from '../../actions/notificationsActions';

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

    readNotification() {
        
    }

    render() { 
        let notifications = [];

        if (this.props.notifications && Array.isArray(this.props.notifications) && this.props.notifications.length > 0) {
            this.props.notifications.forEach(notification => {
                notifications.push(
                    <Notification
                        key={notification.notificationId}
                        id={notification.notificationId}
                        text={notification.text}
                        read={notification.read}
                        //date={moment(notification.createdAt).format("ddd, Do MMM YYYY - HH:mm")}
                        readNotification = {this.readNotification}
                    />
                );                                
            });
        } else {
            notifications = <div className="empty-notifications">There are currently no notifications</div>;            
        }

        return (
            <div className="notification-wrapper">                  
                <div className={"notification-badge visible"} onClick={this.onProfileClick}>{this.props.unreadNotificationsCount}</div>
                <div className={"notification-panel " + this.state.notificationPanelVisibility}>
                    <div className="notification-panel-header clearfix">
                        <strong>Notifications</strong>
                        <p>Mark All as Read</p>
                    </div>
                    <div className="notification-panel-content">
                        {notifications}
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
      fetchNotifications: () => dispatch(fetchNotifications())       
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(NotificationPanel);