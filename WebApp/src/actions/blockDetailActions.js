import { SHOW_DETAIL, HIDE_DETAIL } from '../constants/actionTypes';

export function showDetail(top, left) {
  return {
    type: SHOW_DETAIL,
    payload: {
      top,
      left,
    },
  };
}

export function hideDetail() {
  return {
    type: HIDE_DETAIL,
  };
}
