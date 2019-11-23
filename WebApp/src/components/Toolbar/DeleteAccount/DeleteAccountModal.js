import React, { Component, useState } from "react";
import styled from "styled-components";
import Button from "@material-ui/core/Button";
import { TextField, Modal } from '@material-ui/core';
import axios from "axios";
import "./DeleteAccountModal.scss";

const CancelAccountButton = styled(Button)`
  && {
    background-color: #fafafa;
    color: #666;
    border: 1px solid #666;
    box-shadow: none;
    font-size: x-small;
    padding: 5px;
    color: #2196f3;
    margin-top: 5px;
    line-height: 0;
    border: 1px solid whitesmoke;
    :hover {
      background-color: #fff;
    }
  }
`;

export default class DeleteAccountModal extends Component {
  constructor(props) {
    super();

    this.state = {
      email: props.email,
      password: "",
      submitted: false,
      success: false,
      serverErrors: ""
    };

    this.onLogout = props.onLogout;

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleChange(e) {
    let target = e.target;
    let value = target.value;
    let name = target.name;

    this.setState({
      [name]: value
    });
  }

  handleSubmit(e) {
    e.preventDefault();
    
    const body = {
      email: this.state.email,
      password: this.state.password
    };

    axios({
      method: "post",
      url: "/api/user/deleteUser",
      data: body
    })
      .then(() => {
        this.onLogout();
      })
      .catch(error => {
        if (error.response.status === 403) {
          this.setState({ serverErrors: error.response.data });
        } else this.setState({ serverErrors: error.response.data.error });
      });
  }

  Dialog(props) {
    const [show, setShow] = useState(false);

    const component = props.component;

    const handleClose = () => {
      setShow(false);
    };
    const handleShow = () => {
      setShow(true);
    };

    return (
      <div>
        <CancelAccountButton variant="contained" onClick={handleShow}>
          Zrušiť účet
        </CancelAccountButton>
        <Modal
          open={show}
          aria-labelledby="spring-modal-title"
          aria-describedby="spring-modal-description"
          className={"modal-window"}
          onClose={handleClose}
        >
          <div className={"FormCenter paper"}>
            <p>Pre zrušenie účtu zadajte prosím vaše heslo:</p>
            <form className="FormFields" autoComplete="off" onSubmit={component.handleSubmit} >
              <div className="FormField">
                <TextField
                  label="Heslo"
                  type="password"
                  autoComplete="new-password"
                  required
                  name="password"
                  className="FormField__Label"
                  value={props.state.password}
                  onChange={component.handleChange}
                  fullWidth
                  autoFocus={true}
                />
              </div>
              <div className="FormField">
                <button className="FormField__Button">Potvrdiť</button>
              </div>
            </form>
          </div>
        </Modal>
      </div>
    );
  }

  render() {
    return <this.Dialog component={this} state={this.state} />;
  }
}
