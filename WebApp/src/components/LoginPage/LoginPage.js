import React, { Component } from 'react';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import { ElevatedBox, MacBackground } from '../';

export default class LoginPage extends Component {
  render() {
    return (
      <MacBackground>
        <ElevatedBox>
          <div>
            <TextField label="Meno" />
            <TextField label="Heslo" />
            <Button>Prihlásiť</Button>
          </div>
        </ElevatedBox>
      </MacBackground>
    );
  }
}
