import React, { Component } from 'react';
import classNames from 'classnames';
import axios from 'axios';
import '../RegisterPage/RegisterPage.scss';
import FormValidator from '../FormValidator/FormValidator';
import { Button, TextField}   from '@material-ui/core';
import { Link } from 'react-router-dom';
import { ElevatedBox, MacBackground } from '../';
import { HOME } from '../../util/routes';

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
  }
]);
class ForgotPasswordPage extends Component {
  state = {
    email: '',    
    validation: validator.valid(),
    submitted: false,
    success: false,
    serverErrors: ''
  };

  handleInputChange = event => {
    event.preventDefault();
    this.setState({ [event.target.name]: event.target.value });
  };

  onSubmit = () => {
    const data = {      
      email: this.state.email      
    };

    const validation = validator.validate(data);
    this.setState({ validation, submitted: true });

    if (validation.isValid) {
      axios({
        method: 'post',
        url: '/api/user/resetPassword',
        data
      })
        .then(() => {
          this.setState({ success: true });
      })
        .catch(error => {          
          this.setState({ serverErrors : error.response.data.error });
      });
    }
  }

  render() {
    let validation = this.state.submitted
      ? validator.validate(this.state)
      : this.state.validation;

    const formStyle = this.state.success ? {display: 'none'} : {}
    const messageStyle = !this.state.success ? {display: 'none'} : {}

    return (
      <MacBackground>
        <ElevatedBox>        
          <div className='register-form' style={formStyle}>
            Zabudnuté heslo
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
            <Button
              color="primary"
              variant="contained"
              onClick={this.onSubmit}
            >
              Resetovať
            </Button>            
            <div className={classNames({ 'server-error': this.state.submitted && this.state.serverErrors.length > 0 })}>
                {this.state.serverErrors}
            </div>
          </div>
          <div style={messageStyle}>
            <p>Na zadanú emailovú adresu bol zaslaný email pre obnovenie hesla.</p>
            <Button 
              variant="text"
              size="small"
              component={Link} to={HOME}
            >
              Domov
            </Button>
          </div>
        </ElevatedBox>
      </MacBackground>
    );
  }
}

export default ForgotPasswordPage;
