import api from '../api/api';

import {
	NOTIFICATIONS_FETCH_START,
	NOTIFICATIONS_FETCH_FAIL,
	NOTIFICATIONS_FETCH_DONE
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
