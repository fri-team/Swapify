import axios from "axios";

const API = "/api/";
const EXCHANGES = "exchange/";
const USER_WAITING_EXCHANGES = "userWaitingExchanges/";

export default {    
    exchangeRequests: {
        getAllWaiting: (studentId) => axios.post(
            API + EXCHANGES + USER_WAITING_EXCHANGES,
            `"${studentId}"`,
            {
                headers: {
                    'Content-Type': 'application/json'
                }
            }
        )
    }
}