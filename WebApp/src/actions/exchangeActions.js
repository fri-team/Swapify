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
        studentId = getstate().user.studentId;
        api.exchangeRequests.getAllWaiting(studentId)
            .then(requests => {
                dispatch(loadExchangeRequestsDone(requests))
            })
            .catch(() => { 
                dispatch({
                    type: LOAD_EXCHANGE_REQUESTS_FAIL
                })
            })
    }
}

function loadExchangeRequestsDone(exchangeRequests) {
    return {
        type: LOAD_EXCHANGE_REQUESTS_DONE,
        payload: {
            exchangeRequests
        }
    }
}
