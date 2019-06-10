import {
	NOTIFICATIONS_FETCH_START,
	NOTIFICATIONS_FETCH_DONE,
	NOTIFICATIONS_FETCH_FAIL,	
	NOTIFICATION_READ_CHANGED
} from '../constants/actionTypes';

const initState = {
	isFetching: false,
	error: null,
	notifications: [],	
};

export default function notificationReducer(state = initState, { type, payload }) {
	switch (type) {
		case NOTIFICATIONS_FETCH_START:
			return {
				...state,
				isFetching: true,
			};
		case NOTIFICATIONS_FETCH_DONE:
			return {
				...state,
				isFetching: false,
				error: null,
				notifications: payload.notifications				
			};
		case NOTIFICATIONS_FETCH_FAIL:
			return {
				...state,
				isFetching: false,
				error: payload.error,
				notifications: []				
			};
		case NOTIFICATION_READ_CHANGED:
			return {
				...state,
				notifications: setNotificationRead(state.notifications, payload.notificationId, payload.read)
			}
		default:
			return state;
	}
}

function setNotificationRead(notifications, notificationId, read)
{	
	var nextNotifications = notifications.map(notification => {
		if (notification.notificationId == notificationId) {
			return {
				...notification,
				read
			}
		}

		return notification;
	});

	return nextNotifications;
}