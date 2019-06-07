import {
	NOTIFICATIONS_FETCH_START,
	NOTIFICATIONS_FETCH_DONE,
	NOTIFICATIONS_FETCH_FAIL	
} from '../constants/actionTypes';

const initState = {
	isFetching: false,
	error: null,
	notifications: [],
	unreadNotificationsCount: 0
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
				notifications: payload.notifications,
			};
		case NOTIFICATIONS_FETCH_FAIL:
			return {
				...state,
				isFetching: false,
				error: payload.error,
				notifications: [],
			};
		default:
			return state;
	}
}
