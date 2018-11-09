import reducer, { initState } from './userReducer';
import { login } from '../actions/userActions';
import { addDays } from '../util';

describe('Reducers: User', () => {
  it('Do not set user as logged-in when token is expired', () => {
    const action = login({
      token: 'jwt-token',
      validTo: addDays(new Date(), -1).toString()
    });
    expect(reducer(initState, action)).toEqual(initState);
  });

  it('Set user as logged-in when token is not expired', () => {
    const validTo = addDays(new Date(), 1).toString();
    const action = login({ token: 'jwt-token', validTo });
    const expected = {
      isAuthenticated: true,
      token: 'jwt-token',
      validTo: new Date(validTo)
    };
    expect(reducer(initState, action)).toEqual(expected);
  });
});
