import { combineReducers } from "redux";
import { connectRouter } from "connected-react-router";
import { persistReducer, createTransform } from "redux-persist";
import createEncryptor from "redux-persist-transform-encrypt";
import storage from "redux-persist/lib/storage";
import timetable from "./timetableReducer";
import blockDetail from "./blockDetailReducer";
import exchangeRequests from "./exchangeRequestsReducer";
import user, { getUserData } from "./userReducer";
import notifications from "./notificationReducer";
import toolbar from "./toolbarReducer";
import block from "../redux/slices/block";

const transforms = [
  createTransform(
    ({ validTo, ...state }) => ({
      ...state,
      validTo: validTo ? validTo.toString() : null,
    }),
    getUserData,
    { whitelist: ["user"] }
  ),
];

if (process.env.NODE_ENV == "production") {
  transforms.push(
    createEncryptor({ secretKey: "pNJ7LrUA21b5&jUI6m9PG7*%#v5*sRG$" })
  );
}

const rootPersistConfig = {
  key: "root",
  storage,
  transforms,
  whitelist: ["user"],
};

const rootReducer = (history) =>
  persistReducer(
    rootPersistConfig,
    combineReducers({
      router: connectRouter(history),
      timetable,
      blockDetail,
      user,
      exchangeRequests,
      notifications,
      toolbar,
      block,
    })
  );

export default rootReducer;
