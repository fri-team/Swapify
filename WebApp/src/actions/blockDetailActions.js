import { SHOW_DETAIL, HIDE_DETAIL } from '../constants/actionTypes';

export function showDetail(top, left, course) {
  return {
    type: SHOW_DETAIL,
    payload: {
      top,
      left,
      course,
    },
  };
}

export function hideDetail() {
  return {
    type: HIDE_DETAIL,
  };
}
