import React, { Component } from 'react';
import classNames from 'classnames';
import axios from 'axios';
import '../RegisterPage/RegisterPage.scss';
import FormValidator from '../FormValidator/FormValidator';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import { ElevatedBox, MacBackground } from '../';
import { connect } from 'react-redux';
import { login as loginAction } from '../../actions/userActions';

const validator = new FormValidator([
  {
    field: 'email',
    method: 'isEmpty',
    validWhen: false,
    message: 'Zadajte email'
  },
  {
    field: 'email',
    method: 'isEmail',
    validWhen: true,
    message: 'Emailová adresa nie je platná.'
  },
  {
    field: 'password',
    method: 'isEmpty',
    validWhen: false,
    message: 'Zadajte heslo'
  }
]);
class LoginPage extends Component {
  state = {
    email: '',
    password: '',
    validation: validator.valid(),
    submitted: false,
    serverErrors: ''
  };

  handleInputChange = event => {
    event.preventDefault();
    this.setState({ [event.target.name]: event.target.value });
  };

  onSubmit = () => {
    const { dispatch, history } = this.props;

    const body = {
      email: this.state.email,
      password: this.state.password
    };
    const validation = validator.validate(body);
    this.setState({ validation, submitted: true });

    if (validation.isValid) {
      axios({
        method: 'post',
        url: '/api/user/login',
        data: body
      })
        .then(({ data }) => {
          dispatch(loginAction(data));
          history.push('/timetable');
        })
        .catch(error => {
          this.setState({ serverErrors: error.response.data.error });
        });
    }
  };

  render() {
    let validation = this.state.submitted
      ? validator.validate(this.state)
      : this.state.validation;

    return (
      <MacBackground>
        <ElevatedBox>
          <div className="register-form">
            <TextField
              label="Email"
              required
              name="email"
              error={!!validation.email.message}
              helperText={validation.email.message}
              value={this.state.email}
              fullWidth
              onChange={this.handleInputChange}
            />

            <div className="register-form-spacer">
              <TextField
                label="Heslo"
                type="password"
                required
                name="password"
                error={!!validation.password.message}
                helperText={validation.password.message}
                value={this.state.password}
                fullWidth
                onChange={this.handleInputChange}
              />
            </div>

            <Button color="primary" variant="contained" onClick={this.onSubmit}>
              Prihlásiť
            </Button>
            <div
              className={classNames({
                'server-error':
                  this.state.submitted && this.state.serverErrors.length > 0
              })}
            >
              {this.state.serverErrors}
            </div>
          </div>
        </ElevatedBox>
      </MacBackground>
    );
  }
}

export default connect()(LoginPage);
