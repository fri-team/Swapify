import { LOGIN, LOGOUT, RENEW } from '../constants/actionTypes';

export const login = payload => ({
  type: LOGIN,
  payload
});

export const logout = () => ({
  type: LOGOUT
});

export const renew = payload => ({
  type: RENEW,
  payload
});
