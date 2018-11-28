import React, { Component } from 'react';
import { connect } from 'react-redux';
import axios from 'axios';
import { ElevatedBox, MacBackground } from '../';
import { CircularProgress, Button } from '@material-ui/core';
import { Link } from 'react-router-dom';
import { HOME } from '../../util/routes';

class ConfirmEmailPage extends Component {
  state = {
    confirmationResult: ''
  };

  render() {   
    return (
      <MacBackground>
        <ElevatedBox>
          { 
            this.state.confirmationResult === '' ?
                <CircularProgress  />
            : null 
          }          
          <div>
            <p>{ this.state.confirmationResult }</p>
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

  componentDidMount() {
    const splitedPath = window.location.pathname.split("/");
    const userId = splitedPath[2];
    const token = splitedPath[3];
    const data = {
      userId: userId,
      token: token
    };
    
    axios({
      method: 'post',
      url: '/api/user/confirmEmail',
      data
    })
    .then(() => {      
      this.setState({confirmationResult: "Emailová adresa bola potvrdená" })
    })
    .catch(() => {      
      this.setState({confirmationResult: "Pri potvrdzovaní emailovej adresy nastala chyba" })
    });
  }
}

export default connect()(ConfirmEmailPage);