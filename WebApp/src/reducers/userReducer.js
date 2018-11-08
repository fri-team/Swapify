import { LOGIN_DONE } from '../constants/actionTypes';

export const initState = {
  isAuthenticated: false,
  token: null,
  validTo: null
};

export const setStateIfNotExpired = payload => {
  const validTo = new Date(payload.validTo || 0);
  if (validTo.getTime() < Date.now()) return initState;
  return {
    isAuthenticated: true,
    token: payload.token,
    validTo
  };
};

export default function userReducer(state = initState, { type, payload }) {
  switch (type) {
    case LOGIN_DONE:
      return setStateIfNotExpired(payload);
    default:
      return state;
  }
}
