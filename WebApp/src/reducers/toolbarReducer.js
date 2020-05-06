import {
    MESSAGE_CHANGED,
    SEND_FEEDBACK_SUCCESS,
    SEND_FEEDBACK_FAIL
} from '../constants/actionTypes';

const INITIAL_STATE = { 
    message: ''
};

export default (state = INITIAL_STATE, action) => {
    switch (action.type) {
        case MESSAGE_CHANGED:
            return {
                ...state,
                message: action.payload
            };
        case SEND_FEEDBACK_SUCCESS:
            return {
                ...state,
                message: 'Podarilo sa'
                };
        case SEND_FEEDBACK_FAIL:
            return {
                ...state,
                message: 'Nepodarilo sa'
            };
        default:
            return state;
    }
};