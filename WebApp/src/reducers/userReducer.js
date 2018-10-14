import { LOGIN_DONE } from '../constants/actionTypes';

const initState = {
  isAuthenticated: false,
  token: null,
  validTo: null
};

export const setStateIfNotExpired = state => {
  const validTo = new Date(state.validTo || 0);
  if (validTo.getTime() < Date.now()) return initState;
  return {
    isAuthenticated: true,
    token: state.token,
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
