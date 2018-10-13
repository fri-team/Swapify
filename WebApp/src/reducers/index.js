import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import timetable from './timetableReducer';
import blockDetail from './blockDetailReducer';
import user from './userReducer';

const rootReducer = combineReducers({
  routing: routerReducer,
  timetable,
  blockDetail,
  user
});

export default rootReducer;
