import { call, put, take } from 'redux-saga/effects';
import { delay } from 'redux-saga';
import { routerActions } from 'react-router-redux';
import { REHYDRATE } from 'redux-persist';
import { LOGIN } from '../constants/actionTypes';
import { login, logout } from '../actions/userActions';
import { userLogout } from './userSagas';
import { addDays } from '../util';
import { HOME, TIMETABLE } from '../util/routes';

describe('Sagas: User', () => {
  it('On successful login should redirect to timetable and schedule logout', () => {
    const gen = userLogout();

    const now = new Date('2020-01-01T10:00:00.000Z');
    global.Date.now = jest.fn(() => now.getTime());
    const loginAction = login({ validTo: addDays(now, 1).toISOString() });

    expect(gen.next().value).toEqual(take([LOGIN, REHYDRATE]));
    expect(gen.next(loginAction).value).toEqual(
      call(routerActions.push, TIMETABLE)
    );
    expect(gen.next().value).toEqual(call(delay, 24 * 60 * 60 * 1000));
    expect(gen.next().value).toEqual(put(logout()));
    expect(gen.next().value).toEqual(call(routerActions.push, HOME));
    expect(gen.next()).toEqual({ done: true, value: undefined });
  });

  it('On successful rehydrate should redirect to timetable and schedule logout', () => {
    const gen = userLogout();

    const now = new Date('2020-01-01T10:00:00.000Z');
    global.Date.now = jest.fn(() => now.getTime());
    const rehydrateAction = {
      type: REHYDRATE,
      payload: { user: { validTo: addDays(now, 1).toISOString() } }
    };

    expect(gen.next().value).toEqual(take([LOGIN, REHYDRATE]));
    expect(gen.next(rehydrateAction).value).toEqual(
      call(routerActions.push, TIMETABLE)
    );
    expect(gen.next().value).toEqual(call(delay, 24 * 60 * 60 * 1000));
    expect(gen.next().value).toEqual(put(logout()));
    expect(gen.next().value).toEqual(call(routerActions.push, HOME));
    expect(gen.next()).toEqual({ done: true, value: undefined });
  });
});
