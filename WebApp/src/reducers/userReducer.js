import axios from 'axios';
import { LOGIN, LOGOUT, RENEW } from '../constants/actionTypes';

export const initState = {
  isAuthenticated: false,
  token: null,
  validTo: null
};

export const getUserData = payload => {
  const validTo = new Date(payload.validTo || 0);
  if (validTo.getTime() < Date.now()) return initState;
  axios.defaults.headers.common['Authorization'] = payload.token;
  return {
    userName: payload.userName,
    email: payload.email,
    name: payload.name,
    surname: payload.surname,
    isAuthenticated: true,
    token: payload.token,
    validTo
  };
};

const getRenewData = (state, payload) => {
  const { token, validTo } = getUserData(payload);
  return { ...state, token, validTo };
};

export default function userReducer(state = initState, { type, payload }) {
  switch (type) {
    case LOGIN:
      return getUserData(payload);
    case RENEW:
      return getRenewData(state, payload);
    case LOGOUT:
      return initState;
    default:
      return state;
  }
}
