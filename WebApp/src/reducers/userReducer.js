import { LOGIN_DONE } from '../constants/actionTypes';

export const initState = {
  isAuthenticated: false,
  token: null,
  validTo: null
};

export default function userReducer(state = initState, { type, payload }) {
  switch (type) {
    case LOGIN_DONE:
      return {
        ...state,
        isAuthenticated: true,
        token: payload.token,
        validTo: payload.validTo
      };
    default:
      return state;
  }
}
