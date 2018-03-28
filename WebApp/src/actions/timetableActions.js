import {
  SHOW_SUBJECT,
  HIDE_SUBJECT,
} from '../constants/actionTypes';

export function showSubject(subject) {
  return {
    type: SHOW_SUBJECT,
    payload: { subject },
  };
}

export function hideSubject(subject) {
  return {
    type: HIDE_SUBJECT,
    payload: { subject },
  };
}
