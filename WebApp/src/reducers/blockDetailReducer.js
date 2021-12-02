import { SHOW_DETAIL, HIDE_DETAIL } from '../constants/actionTypes';

const initState = {
  isVisible: false,
  top: 0,
  left: 0,
  course: {},
};

export default function blockDetailReducer(state = initState, { type, payload }) {
  switch (type) {
    case SHOW_DETAIL:
      return {
        isVisible: true,
        top: payload.top,
        left: payload.left,
        course: payload.course,
      };
    case HIDE_DETAIL:
      return {
        ...state,
        isVisible: false,
        course: {},
      }
    default:
      return state;
  }
}
