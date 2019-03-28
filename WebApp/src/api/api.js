import axios from "axios";

const API = "/api/";
const EXCHANGES = "exchange/";
const USER_WAITING_EXCHANGES = "userWaitingExchanges/";

export default {    
    exchangeRequests: {
        getAllWaiting: (userEmail) => axios.get(
            API + EXCHANGES + USER_WAITING_EXCHANGES + userEmail                        
        )
    }
}