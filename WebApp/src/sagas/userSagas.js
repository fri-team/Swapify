import { delay } from 'redux-saga';
import { call, put, take } from 'redux-saga/effects';
import { routerActions } from 'react-router-redux';
import { REHYDRATE } from 'redux-persist';
import { LOGIN } from '../constants/actionTypes';
import { logout } from '../actions/userActions';
import { getUserData } from '../reducers/userReducer';
import { HOME, TIMETABLE } from '../util/routes';

export function* userLogout() {
  const action = yield take([LOGIN, REHYDRATE]);
  const payload = action.type == LOGIN ? action.payload : action.payload.user;
  const data = getUserData(payload);
  if (data.validTo) {
    yield call(routerActions.push, TIMETABLE);
    const expireDelay = data.validTo.getTime() - Date.now();
    yield call(delay, expireDelay);
    yield put(logout());
    yield call(routerActions.push, HOME);
  }
}
