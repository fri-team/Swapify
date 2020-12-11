import React, { PureComponent } from "react";
import { connect } from "react-redux";
import { login } from "../../actions/userActions";
import TextField from "@material-ui/core/TextField";
import axios from "axios";
import './LoginPage.scss';
import ReCAPTCHA from "react-google-recaptcha";
class LoginPage extends PureComponent {
  constructor() {
    super();

    this.state = {
      email: "",
      password: "",
      submitted: false,
      success: false,
      serverErrors: "",
      emailNotConfirmed: false,
      sendConfirmEmailAgainResult: "",
      resetingPassword: false,
      wrongCredentials: false,
      captchaValue : null
    };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.onChangeCaptcha = this.onChangeCaptcha.bind(this);
  }

  handleChange(e) {
    let target = e.target;
    let value = target.type === "checkbox" ? target.checked : target.value;
    let name = target.name;

    this.setState({
      [name]: value,
      serverErrors: "",
      wrongCredentials: false
    });
  }

  onChangeCaptcha(value) {
    if (value) {
      document.getElementById('captchaLabel').style.display = 'none';
      this.setState({
        captchaValue : value
      });
    }
  }

  changeForm = () => {
    if (this.state.resetingPassword) {
      this.setState({ resetingPassword: false });
    } else {
      this.setState({ resetingPassword: true });
    }
  };

  WrongCredentialsMessage(props) {
    const wrongCredentials = props.wrongCredentials;
    const error = props.errors.error;
    if (wrongCredentials) {
      return (
        <div className="wrongCredentials">
          <p>{error}</p>
        </div>
      );
    }
    return null;
  }

  handleSubmit(e) {
    e.preventDefault();

    if (this.state.captchaValue == null) {
      document.getElementById('captchaLabel').style.display = 'block';
      return;
    } else {
      document.getElementById('captchaLabel').style.display = 'none';
    }

    if (!this.state.resetingPassword) {
      const { dispatch } = this.props;
      this.setState({ emailNotConfirmed: false });

      const body = {
        email: this.state.email,
        password: this.state.password,
        captcha: this.state.captchaValue
      };

      axios({
        method: "post",
        url: "/api/user/login",
        data: body
      })
        .then(({ data }) => {
          dispatch(login(data));
        })
        .catch(error => {
          if (error.response.status === 403) {
            this.setState({ serverErrors: error.response.data });
            this.setState({ emailNotConfirmed: true });
          } else if (error.response.status === 400) {
            this.setState({ serverErrors: error.response.data });
            this.setState({ wrongCredentials: true });
          } else {
            this.setState({ serverErrors: error.response.data.error });
          }
        });
    } else {
      const body = {
        email: this.state.email,
        captcha: this.state.captchaValue
      };

      axios({
        method: "post",
        url: "/api/user/resetPassword",
        data: body
      })
        .then(() => {
          this.setState({ success: true });
        })
        .catch(error => {
          this.setState({ serverErrors: error.response.data.error });
        });
    }
  }

  render() {
    const messageStyle = !this.state.success ? { display: "none" } : {};
    return (
      <div className="FormCenter">
        <this.WrongCredentialsMessage
          wrongCredentials={this.state.wrongCredentials}
          errors={this.state.serverErrors}
        />
        {this.state.sendConfirmEmailAgainResult === "" ? (
          <form onSubmit={this.handleSubmit} className="FormFields">
            <div className="FormField">
              <TextField
                label="E-Mailová adresa"
                type="email"
                required
                name="email"
                className="FormField__Label"
                error={this.state.wrongCredentials}
                value={this.state.email}
                onChange={this.handleChange}
                fullWidth
              />
            </div>
            <div style={messageStyle}>
              <p>
                Na zadanú emailovú adresu bol zaslaný email pre obnovenie hesla.
              </p>
            </div>
            {!this.state.resetingPassword && (
              <div className="FormField">
                <TextField
                  label="Heslo"
                  type="password"
                  required
                  name="password"
                  className="FormField__Label"
                  error={this.state.wrongCredentials}
                  value={this.state.password}
                  onChange={this.handleChange}
                  fullWidth
                />
              </div>
            )}

            <div className="FormField captchaClass">
              <ReCAPTCHA
                sitekey="6LeJhgIaAAAAAAyNiupTgRYPQGEOCQc7WvvzR8ue"
                onChange={this.onChangeCaptcha}
              />
              <p id="catpchaText">
                Táto stránka je chránená pomocou služby ReCAPTCHA a Google
                <a href="https://policies.google.com/privacy"> Zásadou ochrany osobných údajov</a> a
                <a href="https://policies.google.com/terms"> Podmienkami služieb</a> ,ktoré sú uplatnené.
              </p>
              <label id='captchaLabel'>Prosím vyplňte že nie ste robot !</label>
            </div>

            <div className="FormField">
              <button className="FormField__Button">
                {!this.state.resetingPassword
                  ? "Prihlásiť sa"
                  : "Resetovať heslo"}
              </button>
            </div>

            <div className="FormField">
              <a onClick={this.changeForm} className="FormField__Link">
                {!this.state.resetingPassword
                  ? "Ak si zabudol heslo, klikni na tento link"
                  : " Späť na login"}
              </a>
            </div>
          </form>
        ) : (
          <p>{this.state.sendConfirmEmailAgainResult}</p>
        )}
      </div>
    );
  }
}

export default connect()(LoginPage);
