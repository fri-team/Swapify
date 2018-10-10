import { combineReducers } from 'redux';
import { routerReducer } from 'react-router-redux';
import timetable from './timetableReducer';
import blockDetail from './blockDetailReducer';

const rootReducer = combineReducers({
  routing: routerReducer,
  timetable,
  blockDetail,
});

export default rootReducer;
