import { connectedRouterRedirect } from 'redux-auth-wrapper/history4/redirect';
import { LoadingPage } from '../components';

export const onlyAuth = connectedRouterRedirect({
  redirectPath: '/login',
  authenticatedSelector: state => state.user.isAuthenticated,
  AuthenticatingComponent: LoadingPage,
  wrapperDisplayName: 'UserIsAuthenticated'
});

export const onlyNotAuth = connectedRouterRedirect({
  redirectPath: '/timetable',
  allowRedirectBack: false,
  authenticatedSelector: state => !state.user.isAuthenticated,
  AuthenticatingComponent: LoadingPage,
  wrapperDisplayName: 'UserIsNotAuthenticated'
});
