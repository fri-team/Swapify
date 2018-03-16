import { concat, without } from 'lodash';
import {
  SHOW_SUBJECT,
  HIDE_SUBJECT,
} from '../constants/actionTypes';
import data from './data.json';

const initState = data;

window.state = initState;

export default function fuelSavingsReducer(state = initState, { type, payload }) {
  switch (type) {
    case SHOW_SUBJECT:
      return {
        ...state,
        showSubjects: concat(state.showSubjects, payload.subject),
      };
    case HIDE_SUBJECT:
      return {
        ...state,
        showSubjects: without(state.showSubjects, payload.subject),
      };
    default:
      return state;
  }
}
