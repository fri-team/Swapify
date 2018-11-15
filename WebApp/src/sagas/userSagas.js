import axios from 'axios';
import _ from 'lodash';
import { delay } from 'redux-saga';
import { call, put, race, take } from 'redux-saga/effects';
import { REHYDRATE } from 'redux-persist';
import { LOGIN, LOGOUT } from '../constants/actionTypes';
import { logout, renew } from '../actions/userActions';

const BEFORE_EXPIRE = 30 * 1000;

const getActionPayload = action => {
  if (action.type == LOGIN) {
    return _.get(action, 'payload');
  } else if (action.type == REHYDRATE) {
    return _.get(action, 'payload.user');
  }
  return null;
};

const parsePayload = payload => {
  const validTo = new Date(_.get(payload, 'validTo', null));
  const token = _.get(payload, 'token');
  if (Date.now() < validTo.getTime()) {
    return { validTo, token };
  }
  return {};
};

const getExpirationDelay = ({ validTo }) => {
  if (_.isDate(validTo)) {
    return validTo.getTime() - Date.now() - BEFORE_EXPIRE;
  }
  return 0;
};

export function* fetchNewToken(token) {
  try {
    const { data } = yield call(axios.post, '/api/user/renew', { token });
    yield put(renew(data));
    return parsePayload(data);
  } catch (err) {
    return null;
  }
}

export function* userLogout() {
  // eslint-disable-next-line
  while (true) {
    const action = yield take([LOGIN, REHYDRATE]);
    let data = parsePayload(getActionPayload(action));
    while (_.get(data, 'validTo')) {
      const expireDelay = getExpirationDelay(data);
      const { expired } = yield race({
        expired: call(delay, expireDelay),
        logout: take(LOGOUT)
      });
      if (expired) {
        data = yield call(fetchNewToken, data.token);
      } else {
        data = null;
        yield put(logout());
      }
    }
  }
}
