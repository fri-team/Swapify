import axios from "axios";
import { API_URL } from '../constants/environments';

const API = "/api/";
const EXCHANGES = "exchange/";
const USER_WAITING_EXCHANGES = "userWaitingExchanges/";

export default {
    exchangeRequests: {
        getAllWaiting: (studentId) => axios.post(
            API_URL + API + EXCHANGES + USER_WAITING_EXCHANGES,
            `"${studentId}"`,
            {
                headers: {
                    'Content-Type': 'application/json'
                }
            }
        )
    },
    notifications: {
        getMine:  (email) =>
            axios.get(API_URL + 'api/notification/' + email, {
                contentType: 'application/json',
                responseType: 'json'
            }),
        setRead: (notificationId, read) =>
            axios.put(API_URL + `api/notification/${notificationId}/${read}`)

    }
}
