import axios from "axios";

const API = "/api/"
const USER_WAITING_EXCHANGES = "userWaitingExchanges/";

export default {    
    exchangeRequests: {
        getAllWaiting: (studentId) => axios({
            method: 'get',
            url: API + USER_WAITING_EXCHANGES,
            body: studentId
        })
    }
}