import React, { Component } from 'react';
import _ from 'lodash';
import classNames from 'classnames';
import axios from 'axios';
import '../RegisterPage/RegisterPage.scss';
import FormValidator from '../FormValidator/FormValidator';
import PropTypes from 'prop-types';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import { ElevatedBox, Form, MacBackground } from '../';
import { Link } from 'react-router-dom';
import { LOGIN } from '../../util/routes';

const validator = new FormValidator([    
    {
      field: 'password',
      method: 'isEmpty',
      validWhen: false,
      message: 'Zadajte heslo'
    },
    {
      field: 'password',
      method: (_, state) => state.password.length > 7,
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
      method: (confirmation, state) => state.password === confirmation,
      validWhen: true,
      message: 'Heslá sa nezhodujú'
    }
  ]);
  class SetNewPasswordPage extends Component {
    state = {
      password: '',
      passwordAgain: '',
      validation: validator.valid(),
      submitted: false,
      success: false,
      serverErrors: []
    };
  
    handleInputChange = event => {
      event.preventDefault();
      this.setState({ [event.target.name]: event.target.value });
    };
  
    onSubmit = () => {
      const splitedPath = window.location.pathname.split("/");
      const userId = splitedPath[2];
      const token = splitedPath[3];

      const data = {
        userId: userId,
        token: token,
        password: this.state.password,
        passwordAgain: this.state.passwordAgain
      };
  
      const validation = validator.validate(data);
      this.setState({ validation, submitted: true });
  
      if (validation.isValid) {
        axios({
          method: 'post',
          url: '/api/user/setNewPassword',
          data
        })
          .then(() => {
            this.setState({success: true })
          })
          .catch(error => {
            var serverErrors = _.flatten(_.values(error.response.data.errors));
            this.setState({ serverErrors });
          });
      }
    };
  
    render() {
      let validation = this.state.submitted
        ? validator.validate(this.state)
        : this.state.validation;
  
      const serverErrors = this.state.serverErrors;
      const serverErrorsList = serverErrors.map(e => <li key={e}>{e}</li>);
  
      return (
        <MacBackground>
          <ElevatedBox>
          { 
            this.state.success === true ?
                <div>
                    Heslo bolo úspešne resetované.
                    <Button 
                      variant="text"
                      size="small"
                      component={Link} to={LOGIN}
                    >
                      Prihlásenie
                    </Button>
                </div>
            : 
            <Form className="register-form" onSubmit={this.onSubmit}>
              Reset hesla
              <div className="register-form-spacer">
                <TextField
                  label="Nové heslo"
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
  
              <div className="register-form-spacer">
                <TextField
                  label="Potvrdenie hesla"
                  type="password"
                  required
                  name="passwordAgain"
                  error={!!validation.passwordAgain.message}
                  helperText={validation.passwordAgain.message}
                  value={this.state.passwordAgain}
                  fullWidth
                  onChange={this.handleInputChange}
                />
              </div>
  
              <Button type="submit" color="primary" variant="contained">
                Resetovať
              </Button>
              <div
                className={classNames({
                  'server-error':
                    this.state.submitted && this.state.serverErrors.length > 0
                })}
              >
                <ul>{serverErrorsList}</ul>
              </div>
            </Form>
          }            
          </ElevatedBox>
        </MacBackground>
      );
    }
  }
  
  SetNewPasswordPage.propTypes = {
    history: PropTypes.shape({
      push: PropTypes.func
    }).isRequired
  };
  
  export default SetNewPasswordPage;
