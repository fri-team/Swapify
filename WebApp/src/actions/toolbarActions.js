import axios from 'axios';
import {
    MESSAGE_CHANGED,
    SEND_FEEDBACK_SUCCESS,
    SEND_FEEDBACK_FAIL
} from '../constants/actionTypes';

export const messageChanged = (text) => {
    return {
        type: MESSAGE_CHANGED,
        payload: text
    };
};

export function sendFeedback(mail, name, subject, message) {
    return function(dispatch) {
        const body = {
            Email: mail,
            Name: name,
            Subject: subject,
            Body: message
          };
          
        axios({
            method: "post",
            url: "/api/user/sendFeedback",
            data: body
        })
        .then(() => {
            dispatch({type: SEND_FEEDBACK_SUCCESS});
        })
        .catch(error => {
            dispatch({type: SEND_FEEDBACK_FAIL, payload: error});
        });
    } 
}