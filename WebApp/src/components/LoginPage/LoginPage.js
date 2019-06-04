import React, { Component } from 'react';
import { connect } from 'react-redux';
import { login } from '../../actions/userActions';
import TextField from '@material-ui/core/TextField';
import axios from 'axios';
class LoginPage extends Component {
  constructor() {
    super();

    this.state = {
      email: '',
      password: '',
      submitted: false,
      serverErrors: '',
      emailNotConfirmed: false,
      sendConfirmEmailAgainResult: '',
      resetingPassword: false
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(e) {
    let target = e.target;
    let value = target.type === 'checkbox' ? target.checked : target.value;
    let name = target.name;

    this.setState({
      [name]: value
    });
  }

  changeForm = () => {
    if(this.state.resetingPassword) {
      this.setState({ resetingPassword: false });
    }
    else {
      this.setState({ resetingPassword: true });
    }
    
  }

  handleSubmit(e) {
    e.preventDefault();

    const { dispatch } = this.props;
    this.setState({ emailNotConfirmed: false });

    const body = {
      email: this.state.email,
      password: this.state.password
    };

    axios({
      method: 'post',
      url: '/api/user/login',
      data: body
    })
      .then(({ data }) => {
        dispatch(login(data));
      }) 
      .catch(error => {
        if (error.response.status === 403) {
          this.setState({ serverErrors: error.response.data });
          this.setState({ emailNotConfirmed: true });
        }
        else
          this.setState({ serverErrors: error.response.data.error });
      });

  }

  render() {
    return (
      <div className="FormCenter">
      { this.state.sendConfirmEmailAgainResult === ''
      ? <form onSubmit={this.handleSubmit} className="FormFields">
          <div className="FormField">
            <TextField
              label="E-Mailová adresa"
              type="email"
              required
              name="email"
              className="FormField__Label"
              value={this.state.email}
              onChange={this.handleChange}
              fullWidth
            />
          </div>

          { !this.state.resetingPassword &&
          <div className="FormField">
            <TextField
              label="Heslo"
              type="password"
              required
              name="password"
              className="FormField__Label"
              value={this.state.password}
              onChange={this.handleChange}
              fullWidth
            />
          </div>
          }

          <div className="FormField">
            <button className="FormField__Button">
            { !this.state.resetingPassword ?
            "Prihlasiť sa" : "Resetovat Heslo" }
            </button> 
          </div>
          
          <div className="FormField">
          <a onClick={this.changeForm} className="FormField__Link">
              { !this.state.resetingPassword ?
              " Ak si zabudol heslo klinki na tento link" : " Spat na login" }
            </a>
            
          </div>
        </form>
        : <p>{ this.state.sendConfirmEmailAgainResult }</p> 
    }
      </div>
    );
  }
}

export default connect()(LoginPage);
