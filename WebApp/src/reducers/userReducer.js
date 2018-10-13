import { LOGIN_DONE } from '../constants/actionTypes';

export const initState = {
  token: null,
  validTo: null
};

export default function userReducer(state = initState, { type, payload }) {
  switch (type) {
    case LOGIN_DONE:
      return {
        ...state,
        token: payload.token,
        validTo: payload.validTo
      };
    default:
      return state;
  }
}
