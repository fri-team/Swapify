import React, { Component } from 'react';
import '../RegisterPage/RegisterPage.scss';
import { ElevatedBox, MacBackground } from '../';

export default class ConfirmEmailPage extends Component { 
    render() {   
    return (
        <MacBackground>
          <ElevatedBox>        
            <div>
              Potvrdenie emailovej adresy             
            </div>
          </ElevatedBox>
        </MacBackground>
      );
    }
}
