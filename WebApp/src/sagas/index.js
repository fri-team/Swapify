import { all } from 'redux-saga/effects';
import { userLogout } from './userSagas';

export default function* rootSaga() {
  yield all([userLogout()]);
}
