import { LOGIN } from '../constants/actionTypes';

export const initState = {
  isAuthenticated: false,
  token: null,
  validTo: null
};

export const getUserData = payload => {
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
    case LOGIN:
      return getUserData(payload);
    default:
      return state;
  }
}
