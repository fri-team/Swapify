// TODO: remove this "eslint-disable" after you remove all console logs...
/* eslint-disable no-console */
import React, { Component } from 'react';
import axios from 'axios';
import { bind } from '../../util/stateBinding';

export default class RegisterPage extends Component {
  state = {
    email: '',
    password: '',
    passwordCheck: '',
  }

  onSubmit = () => {
    const data = {
      email: this.state.email,
      password: this.state.password,
    };
    console.log(data);
    axios({
      method: 'post',
      url: 'http://localhost:5000/api/user/register',
      data,
    }).then(() => {
      console.log('Api call succeeded');
    }).catch(() => {
      console.error('Api call failed');
    });
  }

  render() {
    return (
      <div>
        <h1>Register Page</h1>
        <label htmlFor="email">E-mail:</label>
        <input
          type="text"
          name="email"
          value={this.state.email}
          onChange={bind('email', this)}
        />
        <br />
        <label htmlFor="password">Heslo:</label>
        <input
          type="password"
          name="password"
          value={this.state.password}
          onChange={bind('password', this)}
        />
        <br />
        <label htmlFor="password-check">Potvrdenie hesla:</label>
        <input
          type="password"
          name="password-check"
          value={this.state.passwordCheck}
          onChange={bind('passwordCheck', this)}
        />
        <br />
        <button onClick={this.onSubmit}>Registruj</button>
      </div>
    );
  }
}
