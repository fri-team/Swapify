import { PureComponent } from "react";
import React from "react";
import './NotificationPanel.scss';

export default class Notification extends PureComponent {                    
    render() {        
        return (
            <div className={"notification clearfix " + this.props.read}>                
                <div className="notification-content">
                    <p>{this.props.text}</p>
                    <small>{this.props.date}</small>
                    <span className="read-button" onClick={this.props.readNotification}></span>
                </div>
            </div>
        );
    }
};
