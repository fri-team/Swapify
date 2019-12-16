import React, { Component } from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';
import classNames from 'classnames';
import FormValidator from '../FormValidator/FormValidator';
import axios from 'axios';
import PropTypes from 'prop-types';
import TextField from '@material-ui/core/TextField';
import { HOME } from '../../util/routes';
import Modal from '../Modal/Modal';
import Backdrop from '../Backdrop/Backdrop';
//import {Document, Page} from 'react-pdf';
//import PrivacyPolicy from '../PrivacyPolicyPage/PrivacyPolicy';

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

class RegisterPage extends Component {
  constructor() {
    super();

    this.state = {
      name: '',
      surname: '',
      email: '',
      password: '',
      passwordAgain: '',
      validation: validator.valid(),
      submitted: false,
      serverErrors: [],
      hasAgreed: false,
      privacyPolicyOpened : false
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }


  handleChange = event => {
    //event.preventDefault();
    this.setState({ [event.target.name]: event.target.value });
  };

  handlePrivacyPolicyOpened = () => {
    this.setState({privacyPolicyOpened: true});
  };

  handleModalCancel = () => {
    this.setState({privacyPolicyOpened: false});
  };

  handleSubmit = event => {
    event.preventDefault();
    const body = {
      name: this.state.name,
      surname: this.state.surname,
      email: this.state.email,
      password: this.state.password,
      passwordAgain: this.state.passwordAgain
    };

    const validation = validator.validate(body);
    this.setState({ validation, submitted: true });

    if (validation.isValid) {
      axios({
        method: 'post',
        url: '/api/user/register',
        data: body
      })
        .then(() => {
          this.props.history.push(HOME);
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
      <React.Fragment>
        {this.state.privacyPolicyOpened && <Backdrop></Backdrop>}
        {this.state.privacyPolicyOpened && <Modal title="Privacy policies" onCancel={this.handleModalCancel}>
          <p>Pomáhať a chrániť!</p>
        </Modal>}
        <div className="FormCenter">
          <form onSubmit={this.handleSubmit} className="FormFields">
            <div className="FormField">
              <TextField
                label="Meno"
                type="text"
                required
                name="name"
                className="FormField__Label"
                error={!!validation.name.message}
                helperText={validation.name.message}
                value={this.state.name}
                onChange={this.handleChange}
                fullWidth
              />
            </div>

            <div className="FormField">
              <TextField
                label="Priezvisko"
                type="text"
                required
                name="surname"
                className="FormField__Label"
                error={!!validation.surname.message}
                helperText={validation.surname.message}
                value={this.state.surname}
                onChange={this.handleChange}
                fullWidth
              />
            </div>

            <div className="FormField">
              <TextField
                label="E-Mailová adresa"
                type="email"
                required
                name="email"
                className="FormField__Label"
                error={!!validation.email.message}
                helperText={validation.email.message}
                value={this.state.email}
                onChange={this.handleChange}
                fullWidth
              />
            </div>

            <div className="FormField">
              <TextField
                label="Heslo"
                type="password"
                required
                name="password"
                className="FormField__Label"
                error={!!validation.password.message}
                helperText={validation.password.message}
                value={this.state.password}
                onChange={this.handleChange}
                fullWidth
              />
            </div>

            <div className="FormField">
              <TextField
                label="Potvrdenie hesla"
                type="password"
                required
                name="passwordAgain"
                className="FormField__Label"
                error={!!validation.passwordAgain.message}
                helperText={validation.passwordAgain.message}
                value={this.state.passwordAgain}
                onChange={this.handleChange}
                fullWidth
              />
            </div>

            <div className="FormField">
              
            <input
                  className="FormField__Checkbox"
                  type="checkbox" name="hasAgreed"
                  value={this.state.hasAgreed}
                  onChange={this.handleChange}
                  required
                />
              <label
                className="FormField__CheckboxLabel"
                //onClick={this.handlePrivacyPolicyOpened}
              >
                
                {'Kliknutím na "Registrovať sa" potvrdzuješ, že si si prečítal(a) a súhlasíš so '}
                <a href="../PrivacyPolicyPage/PrivacyPolicy.js" target="_blank" rel="noopener noreferrer" >Zmluvnými podmienkami a Zásadami ochrany osobných údajov</a>{'.'}
              </label>
            </div>

            <div className="FormField">
              <button className="FormField__Button">
                Registrovať sa
              </button>
            </div>

            <div
              className={classNames({
                'server-error':
                  this.state.submitted && this.state.serverErrors.length > 0
              })}
            >
              <ul>{serverErrorsList}</ul>
            </div>

          </form>
        </div>
      </React.Fragment>
    );
  }
}

RegisterPage.propTypes = {
  history: PropTypes.shape({
    push: PropTypes.func
  }).isRequired
};

export default connect()(RegisterPage);
