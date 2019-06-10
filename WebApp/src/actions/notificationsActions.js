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
        
        let studentId = getState().user.studentId;        

        api.notifications.getMine(studentId)
		.then((response) => {
			dispatch(fetchNotificationsDone(response.data));
		}).catch((error) => {
			dispatch(fetchNotificationsFail(error));
		});
	};
}

export function setRead(notificationId, read) {
	return (dispatch, getState) => {
		let studentId = getState().user.studentId;

		api.notifications.setRead(studentId, notificationId, read)
			.then((response) => {
				dispatch(notificationReadChanged(notificationId, read));
			})
		//TODO catch error 	
	}
}

