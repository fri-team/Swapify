import { delay } from 'redux-saga';
import { call, put, take } from 'redux-saga/effects';
import { REHYDRATE } from 'redux-persist';
import { LOGIN } from '../constants/actionTypes';
import { logout } from '../actions/userActions';
import { getUserData } from '../reducers/userReducer';

export function* userLogout() {
  const action = yield take([LOGIN, REHYDRATE]);
  const payload = action.type == LOGIN ? action.payload : action.payload.user;
  const data = getUserData(payload);
  if (data.validTo) {
    const expireDelay = data.validTo.getTime() - Date.now();
    yield call(delay, expireDelay);
    yield put(logout());
  }
}
