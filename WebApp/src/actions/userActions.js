import { LOGIN_DONE } from '../constants/actionTypes';

export const login = payload => ({
  type: LOGIN_DONE,
  payload
});
