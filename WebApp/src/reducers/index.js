import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import { persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import timetable from './timetableReducer';
import blockDetail from './blockDetailReducer';
import user from './userReducer';

const userPersistConfig = {
  key: 'user',
  storage
};

const rootReducer = combineReducers({
  routing: routerReducer,
  timetable,
  blockDetail,
  user: persistReducer(userPersistConfig, user)
});

export default rootReducer;
