import {
    LOAD_EXCHANGE_REQUESTS_DONE
} from '../constants/actionTypes'

export const initState = {
  exchangeRequests: []
};

export default function exchangeRequestsReducer(state = initState, {type, payload}){
    switch (type) {
        case LOAD_EXCHANGE_REQUESTS_DONE:
            return {
                ...state,
                exchangeRequests = payload.exchangeRequests
            }    
    }
}