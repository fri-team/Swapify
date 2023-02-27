import {
    LOAD_EXCHANGE_REQUESTS,
    LOAD_EXCHANGE_REQUESTS_DONE,
    LOAD_EXCHANGE_REQUESTS_FAIL
} from '../constants/actionTypes';

import api from '../api/api'

export function loadExchangeRequests() {
    return (dispatch, getState) => {
        dispatch({
            type: LOAD_EXCHANGE_REQUESTS
        });
        let timetableId = getState().user.timetableId;
        api.exchangeRequests.getAllWaiting(timetableId)
            .then(response => {
                dispatch(loadExchangeRequestsDone(response.data))
            })
            .catch(() => {
                dispatch({
                    type: LOAD_EXCHANGE_REQUESTS_FAIL
                })
            })
    }
}

export function loadExchangeRequestsDone(exchangeRequests) {
    return {
        type: LOAD_EXCHANGE_REQUESTS_DONE,
        payload: {
            exchangeRequests
        }
    }
}
