import { combineReducers } from 'redux';
import timetable from './timetableReducer';
import { routerReducer } from 'react-router-redux';

const rootReducer = combineReducers({
  routing: routerReducer,
  timetable,
});

export default rootReducer;
