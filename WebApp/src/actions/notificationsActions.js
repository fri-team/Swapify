import api from '../api/api';

import {
	NOTIFICATIONS_FETCH_START,
	NOTIFICATIONS_FETCH_FAIL,
	NOTIFICATIONS_FETCH_DONE,
	NOTIFICATION_READ_CHANGED
} from '../constants/actionTypes';

function fetchNotificationsStart() {
	return {
		type: NOTIFICATIONS_FETCH_START,
	};
}

function fetchNotificationsDone(notifications) {
	return {
		type: NOTIFICATIONS_FETCH_DONE,
		payload: {
			notifications,
		},
	};
}

function fetchNotificationsFail(error) {
	return {
		type: NOTIFICATIONS_FETCH_FAIL,
		payload: {
			error,
		},
	};
}

function notificationReadChanged(notificationId, read)
{
	return {
		type: NOTIFICATION_READ_CHANGED,
		payload: {
			notificationId,
			read
		}
	}
}

export function fetchNotifications() {
    return (dispatch, getState) => {        
        dispatch(fetchNotificationsStart());
        
        let email = getState().user.email;        

        api.notifications.getMine(email)
		.then((response) => {
			dispatch(fetchNotificationsDone(response.data));
		}).catch((error) => {
			dispatch(fetchNotificationsFail(error));
		});
	};
}

export function setRead(notificationId, read) {
	return (dispatch, getState) => {		
		api.notifications.setRead(notificationId, read)
			.then((response) => {
				dispatch(notificationReadChanged(notificationId, read));
			})
		//TODO catch error 	
	}
}

