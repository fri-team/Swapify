import { connectedRouterRedirect } from 'redux-auth-wrapper/history4/redirect';
import { LoadingPage } from '../components';
import { HOME, TIMETABLE } from './routes';

export const onlyAuth = connectedRouterRedirect({
  redirectPath: HOME,
  authenticatedSelector: state => state.user.isAuthenticated,
  AuthenticatingComponent: LoadingPage,
  wrapperDisplayName: 'UserIsAuthenticated'
});

export const onlyNotAuth = connectedRouterRedirect({
  redirectPath: TIMETABLE,
  allowRedirectBack: false,
  authenticatedSelector: state => !state.user.isAuthenticated,
  AuthenticatingComponent: LoadingPage,
  wrapperDisplayName: 'UserIsNotAuthenticated'
});
