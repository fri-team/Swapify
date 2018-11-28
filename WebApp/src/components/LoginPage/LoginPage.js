import React, { Component } from 'react';
import classNames from 'classnames';
import axios from 'axios';
import '../RegisterPage/RegisterPage.scss';
import FormValidator from '../FormValidator/FormValidator';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import { ElevatedBox, Form, MacBackground } from '../';
import { connect } from 'react-redux';
import { login } from '../../actions/userActions';
import { Link } from 'react-router-dom';
import { FORGOTPASSWORD } from '../../util/routes';

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
    const { dispatch } = this.props;

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
          dispatch(login(data));
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
          <Form className="register-form" onSubmit={this.onSubmit}>
            Prihlásenie
            <div className="register-form-spacer">
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
            </div>

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

            <Button type="submit" color="primary" variant="contained">
              Prihlásiť
            </Button>
            <Button 
              variant="text"
              size="small"
              component={Link} to={FORGOTPASSWORD}
            >
              Zabudnuté heslo
            </Button>
            <div
              className={classNames({
                'server-error':
                  this.state.submitted && this.state.serverErrors.length > 0
              })}
            >
              {this.state.serverErrors}
            </div>
          </Form>
        </ElevatedBox>
      </MacBackground>
    );
  }
}

export default connect()(LoginPage);
