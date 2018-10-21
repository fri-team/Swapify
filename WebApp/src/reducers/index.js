import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import { persistReducer, createTransform } from 'redux-persist';
import createEncryptor from 'redux-persist-transform-encrypt';
import storage from 'redux-persist/lib/storage';
import timetable from './timetableReducer';
import blockDetail from './blockDetailReducer';
import user, { setStateIfNotExpired } from './userReducer';

const transforms = [
  createTransform(
    ({ validTo, ...state }) => ({
      ...state,
      validTo: validTo ? validTo.toString() : null
    }),
    setStateIfNotExpired,
    { whitelist: ['user'] }
  )
];

if (process.env.NODE_ENV == 'production') {
  transforms.push(
    createEncryptor({ secretKey: 'pNJ7LrUA21b5&jUI6m9PG7*%#v5*sRG$' })
  );
}

const rootPersistConfig = {
  key: 'root',
  storage,
  transforms,
  whitelist: ['user']
};

const rootReducer = persistReducer(
  rootPersistConfig,
  combineReducers({
    routing: routerReducer,
    timetable,
    blockDetail,
    user
  })
);

export default rootReducer;
