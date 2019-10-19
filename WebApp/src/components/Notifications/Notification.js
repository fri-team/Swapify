import React, { PureComponent } from "react";
import './NotificationPanel.scss';

export default class Notification extends PureComponent {
    
    readNotification = () => {        
        this.props.setNotificationRead(this.props.id, !this.props.read);
    }

    render() {    
        var read = this.props.read ? "read" : "unread";

        return (
            <div className={"notification clearfix " + read}>                
                <div className="notification-content">
                    <p>{this.props.text}</p>
                    <small>{this.props.date}</small>
                    <span className="read-button" onClick={this.readNotification}></span>
                </div>
            </div>
        );
    }
}
