import { combineReducers } from 'redux';
import blocks from './blockReducer';
import { routerReducer } from 'react-router-redux';

const rootReducer = combineReducers({
  blocks,
  routing: routerReducer
});

export default rootReducer;
