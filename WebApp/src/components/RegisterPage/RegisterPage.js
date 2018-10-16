// TODO: remove this "eslint-disable" after you remove all console logs...
/* eslint-disable no-console */

import React, { Component } from 'react';
import classNames from 'classnames';
import axios from 'axios';
import './RegisterPage.scss';
import FormValidator from '../FormValidator/FormValidator';
import PropTypes from 'prop-types';

export default class RegisterPage extends Component {
  constructor(props) {
    super(props);
    
    this.validator = new FormValidator([
      { 
        field: 'email', 
        method: 'isEmpty', 
        validWhen: false, 
        message: 'Email je povinný.' 
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
        message: 'Meno je povinné.'
      },
      { 
        field: 'surname', 
        method: 'isEmpty', 
        validWhen: false, 
        message: 'Priezvisko je povinné.'
      },
      { 
        field: 'password', 
        method: 'isEmpty', 
        validWhen: false, 
        message: 'Heslo je povinné.'
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
        message: 'Potvrdenie hesla je povinné.'
       },
      { 
        field: 'passwordAgain', 
        method: this.passwordMatch,
        validWhen: true, 
        message: 'Heslá sa nezhodujú.'
      }
    ]);

    this.state = {
      name: '',
      surname: '',
      email: '',
      password: '',
      passwordAgain: '',
      validation: this.validator.valid(),
      serverErrors: []
    }
  }

  submitted = false;  
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
    this.setState({ validation });
    this.submitted = true;

    if (validation.isValid) {
      axios({
        method: 'post',
        url: '/api/user/register',
        data,
      }).then(() => {
        this.props.history.push('/');
      }).catch(error => {
        this.setState({ serverErrors: new Array() })
        var newErrorsArray = new Array();
        var dictionary = error.response.data.errors;
        for (var key in dictionary) {
          if (dictionary.hasOwnProperty(key)) {
            for (let i = 0; i < dictionary[key].length; i++) {
              newErrorsArray.push(dictionary[key][i]);
            }
          }
      }
        this.setState({ serverErrors: newErrorsArray });
      });
    }    
  }

  render() {
    let validation = this.submitted ?
                     this.validator.validate(this.state) :
                     this.state.validation

    const serverErrors = this.state.serverErrors;
    const serverErrorsList = serverErrors.map((e) => <li key={e}>{e}</li>);

    return (
      <div className="register-box">
        <h1>Registrácia</h1>        
        <input
          className={classNames({ 'has-error': validation.name.isInvalid })}
          type="text"
          name="name"
          placeholder="Meno"
          value={this.state.name}
          onChange={this.handleInputChange}
        />
        <span className="help-block">{validation.name.message}</span>
        <br />        
        <input
          className={classNames({ 'has-error': validation.surname.isInvalid })}
          type="text"
          name="surname"
          placeholder="Priezvisko"
          value={this.state.surname}
          onChange={this.handleInputChange}
        />
        <span className="help-block">{validation.surname.message}</span>
        <br />
        <input
          className={classNames({ 'has-error': validation.email.isInvalid })}
          type="text"
          name="email"
          placeholder="Email"
          value={this.state.email}
          onChange={this.handleInputChange}
        />
        <span className="help-block" id="emailHelp">{validation.email.message}</span>
        <br />
        <input
          className={classNames({ 'has-error': validation.password.isInvalid })}
          type="password"
          name="password"
          placeholder="Heslo"
          value={this.state.password}
          onChange={this.handleInputChange}
        />
        <span className="help-block">{validation.password.message}</span>
        <br />
        <input
          className={classNames({ 'has-error': validation.passwordAgain.isInvalid })}
          type="password"
          name="passwordAgain"
          placeholder="Potvrdenie hesla"
          value={this.state.passwordAgain}
          onChange={this.handleInputChange}
        />
        <span className="help-block">{validation.passwordAgain.message}</span>
        <br />
        <button onClick={this.onSubmit}>Registrovať</button>
        <div className={classNames({ 'server-error': this.submitted && this.state.serverErrors.length > 0 })}> 
          <ul>{ serverErrorsList }</ul>
        </div>
      </div>
    );
  }
}

RegisterPage.propTypes = {
  history: PropTypes.shape({
    push: PropTypes.func,
  }).isRequired,
}
