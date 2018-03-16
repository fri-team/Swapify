import { combineReducers } from 'redux';
import fuelSavings from './fuelSavingsReducer';
import blocks from './blockReducer';
import { routerReducer } from 'react-router-redux';

const rootReducer = combineReducers({
  fuelSavings,
  blocks,
  routing: routerReducer
});

export default rootReducer;
