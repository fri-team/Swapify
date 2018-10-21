import React, { Component } from 'react';
import axios from 'axios';
import { connect } from 'react-redux';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import { ElevatedBox, MacBackground } from '../';
import { login as loginAction } from '../../actions/userActions';
import './LoginPage.scss';

class LoginPage extends Component {
  state = {
    login: '',
    password: '',
    loginError: '',
    passwordError: '',
    sending: false
  };

  updateValue = key => evt => {
    const { value } = evt.target;
    this.setState({
      [key]: value,
      [`${key}Error`]: value ? '' : 'Hodnota je povinná'
    });
  };

  canLogin = () => {
    const { login, password, sending } = this.state;
    return !sending && login && password;
  };

  login = () => {
    const { dispatch, history } = this.props;
    const { login, password } = this.state;
    this.setState({ sending: true });
    axios
      .post('/api/user/login', { login, password })
      .then(({ data }) => {
        dispatch(loginAction(data));
        history.push('/timetable');
      })
      .catch(() =>
        this.setState({
          login: '',
          password: '',
          loginError: 'Login alebo heslo nie je správne',
          passwordError: 'Login alebo heslo nie je správne',
          sending: false
        })
      );
  };

  render() {
    const { login, password, loginError, passwordError, sending } = this.state;
    return (
      <MacBackground>
        <ElevatedBox>
          <div className="login-form">
            <TextField
              label="Login"
              required
              disabled={sending}
              error={!!loginError}
              helperText={loginError}
              value={login}
              onChange={this.updateValue('login')}
            />
            <div className="login-form-spacer">
              <TextField
                label="Heslo"
                type="password"
                disabled={sending}
                required
                error={!!passwordError}
                helperText={passwordError}
                value={password}
                onChange={this.updateValue('password')}
              />
            </div>
            <Button
              disabled={!this.canLogin()}
              color="primary"
              variant="contained"
              onClick={this.login}
            >
              Prihlásiť
            </Button>
          </div>
        </ElevatedBox>
      </MacBackground>
    );
  }
}

export default connect()(LoginPage);
