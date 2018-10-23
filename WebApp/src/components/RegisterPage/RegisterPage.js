import React, { Component } from 'react';
import {flatten ,values} from 'lodash';
import classNames from 'classnames';
import axios from 'axios';
import './RegisterPage.scss';
import FormValidator from '../FormValidator/FormValidator';
import PropTypes from 'prop-types';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import { ElevatedBox, MacBackground } from '../';

class RegisterPage extends Component {
  static validator = new FormValidator([
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
      field: 'name',
      method: 'isEmpty',
      validWhen: false,
      message: 'Zadajte meno'
    },
    {
      field: 'surname',
      method: 'isEmpty',
      validWhen: false,
      message: 'Zadajte priezvisko'
    },
    {
      field: 'password',
      method: 'isEmpty',
      validWhen: false,
      message: 'Zadajte heslo'
    },
    {
      field: 'password',
      method: this.passwordLength,
      validWhen: true,
      message: 'Heslo musí obsahovať aspoň 8 znakov.'
    },
    {
      field: 'password',
      method: 'matches',
      args: [/\d/],
      validWhen: true,
      message: 'Heslo musí obsahovať aspoň jednu číslicu.'
    },
    {
      field: 'password',
      method: 'matches',
      args: [/.*[A-Z]/],
      validWhen: true,
      message: 'Heslo musí obsahovať aspoň jedno veľké písmeno.'
    },
    {
      field: 'password',
      method: 'matches',
      args: [/.*[a-z]/],
      validWhen: true,
      message: 'Heslo musí obsahovať aspoň jedno malé písmeno.'
    },
    {
      field: 'passwordAgain',
      method: 'isEmpty',
      validWhen: false,
      message: 'Zadajte heslo znova'
      },
    {
      field: 'passwordAgain',
      method: this.passwordMatch,
      validWhen: true,
      message: 'Heslá sa nezhodujú'
    }
  ]);

  state = {
    name: '',
    surname: '',
    email: '',
    password: '',
    passwordAgain: '',
    validation: this.validator.valid(),
    submitted: false,
    serverErrors: []
  }

  passwordMatch = (confirmation, state) => (state.password === confirmation)
  passwordLength = (confirmation, state) => (state.password.length > 7)

  handleInputChange = event => {
    event.preventDefault();

    this.setState({
      [event.target.name]: event.target.value,
    });
  }

  onSubmit = () => {
    const data = {
      name: this.state.name,
      surname: this.state.surname,
      email: this.state.email,
      password: this.state.password,
      passwordAgain: this.state.passwordAgain
    };

    const validation = this.validator.validate(this.state);
    this.setState({ validation, submitted: true });

    if (validation.isValid) {
      axios({
        method: 'post',
        url: '/api/user/register',
        data,
      }).then(() => {
        this.props.history.push('/');
      }).catch(error => {
        var serverErrors = flatten(values(error.response.data.errors));
        this.setState({ serverErrors });
      });
    }
  }

  render() {
    let validation = this.state.submitted ?
                     this.validator.validate(this.state) :
                     this.state.validation;

    const serverErrors = this.state.serverErrors;
    const serverErrorsList = serverErrors.map((e) => <li key={e}>{e}</li>);

    return (
      <MacBackground>
        <ElevatedBox>            
          <div className="register-form">
            <TextField
              label="Meno"
              required
              name = "name"
              error={!!validation.name.message}
              helperText={validation.name.message}
              value={this.state.name}
              fullWidth
              onChange={this.handleInputChange}
            />

            <div className="register-form-spacer">
              <TextField
                  label="Priezvisko"
                  required
                  name = "surname"
                  error={!!validation.surname.message}
                  helperText={validation.surname.message}
                  value={this.state.surname}
                  fullWidth
                  onChange={this.handleInputChange}
              />
            </div>

            <div className="register-form-spacer">
              <TextField
                  label="Email"
                  required
                  name = "email"
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
                  name = "password"
                  error={!!validation.password.message}
                  helperText={validation.password.message}
                  value={this.state.password}
                  fullWidth
                  onChange={this.handleInputChange}
              />
            </div>

            <div className="register-form-spacer">
              <TextField
                  label="Potvrdenie hesla"
                  type="password"
                  required
                  name = "passwordAgain"
                  error={!!validation.passwordAgain.message}
                  helperText={validation.passwordAgain.message}
                  value={this.state.passwordAgain}
                  fullWidth
                  onChange={this.handleInputChange}
              />
            </div>

            <Button
              color="primary"
              variant="contained"
              onClick={this.onSubmit}
            >
              Registrovať
            </Button>
            <div className={classNames({ 'server-error': this.state.submitted && this.state.serverErrors.length > 0 })}>
              <ul>{ serverErrorsList }</ul>
            </div>
          </div>
        </ElevatedBox>
      </MacBackground>
    );
  }
}

RegisterPage.propTypes = {
  history: PropTypes.shape({
    push: PropTypes.func,
  }).isRequired,
}

export default RegisterPage;
