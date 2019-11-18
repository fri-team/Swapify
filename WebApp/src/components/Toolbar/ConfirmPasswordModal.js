import React, { Component, useState } from "react";
import Modal from "react-bootstrap/Modal";
import styled from "styled-components";
import Button from "@material-ui/core/Button";
import TextField from "@material-ui/core/TextField";
import axios from "axios";
import { logout } from "../../actions/userActions";

export default class ConfirmPasswordModal extends Component {
  constructor(props) {
    super();

    this.state = {
      email: props.email,
      password: "",
      submitted: false,
      success: false,
      serverErrors: ""
    };

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

  handleSubmit() {
    const { dispatch } = this.props;

    const body = {
      email: this.state.email,
      password: this.state.password
    };

    axios({
      method: "post",
      url: "/api/user/deleteUser",
      data: body
    })
      .then(({ data }) => {
        dispatch(logout(data));
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
          show={show}
          centered={true}
          keyboard={false}
          onHide={handleClose}
        >
          <Modal.Header closeButton>
            <Modal.Title>Zrušenie účtu</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            Pre zrušenie účtu zadajte prosím vaše heslo:
            <div className="FormCenter">
              <form className="FormFields" onSubmit={component.handleSubmit} autoComplete="false">
                <div className="FormField">
                  <TextField
                    label="Heslo"
                    type="password"
                    autoComplete="off"
                    required
                    name="password"
                    className="FormField__Label"
                    value={props.state.password}
                    onChange={component.handleChange}
                    fullWidth
                  />
                </div>
                <div className="FormField">
                  <button className="FormField__Button">Potvrdiť</button>
                </div>
              </form>
            </div>
          </Modal.Body>
        </Modal>
      </div>
    );
  }

  render() {
    return <this.Dialog component={this} state={this.state} />;
  }
}

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
