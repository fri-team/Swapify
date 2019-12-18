import React, { PureComponent } from 'react';
import Toolbar from '../Toolbar/Toolbar';
import Dialog from '@material-ui/core/Dialog';
import DialogTitle from '@material-ui/core/DialogTitle';
import DialogContent from '@material-ui/core/DialogContent';
//import DialogContentText from '@material-ui/core/DialogContentText';
import FormControl from '@material-ui/core/FormControl';
import TextField from '@material-ui/core/TextField';
import SendIcon from '@material-ui/icons/Send';
import swapifyGIF from '../../images/Swapify-GIF.gif';
import { connect } from 'react-redux';
import TimetableContainer from '../../containers/TimetableContainer/TimetableContainer';
import BlockDetailContainer from '../../containers/BlockDetailContainer/BlockDetailContainer';
import SidebarContainer from '../Sidebar/SidebarContainer';

import './TimetablePage.scss';
import { Button } from '@material-ui/core';

class TimetablePage extends PureComponent {
  state = { 
    sidebarOpen: false, 
    helpModalWindowOpen: false,
    mailUsModalWindowOpen: false,
    user: this.props.user 
  };
  
  render() {
    return (
      <div className="container">
        <Toolbar
          toggleSidebar={() =>
            this.setState(prevState => ({
              sidebarOpen: !prevState.sidebarOpen
            }))
          }
          toggleHelpModalWindow={() =>
            this.setState(prevState => ({
              helpModalWindowOpen: !prevState.helpModalWindowOpen
            }))
          }
        />
        <SidebarContainer
          open={this.state.sidebarOpen}
          onClose={() => this.setState({ sidebarOpen: false })}
          toggleMailUsModalWindow={() =>
            this.setState(prevState => ({
              mailUsModalWindowOpen: !prevState.mailUsModalWindowOpen
            }))
          }
        />
        <TimetableContainer />
        
        <Dialog
          fullWidth={true}
          maxWidth={'lg'}
          open={this.state.helpModalWindowOpen}
          onClose={() => this.setState({ helpModalWindowOpen: false })}
          onBackdropClick={() => this.setState({ helpModalWindowOpen: false })}
          aria-labelledby="max-width-dialog-title"
        >
          <DialogTitle id="max-width-dialog-title">Pomocník</DialogTitle>
          <DialogContent className = "dialogHelpContent">
            <div className = "oneGif">
              <p>Výmena cvičenia</p>
              <img src={swapifyGIF} alt="vymena cvicenia" />
            </div>
            <div className = "oneGif">
              <p>Potvrdenie výmeny</p>
              <img src={swapifyGIF} alt="potvrdenie vymeny" />
            </div>
            <div className = "oneGif">
              <p>Pridanie predmetu</p>
              <img src={swapifyGIF} alt="pridanie predmetu" />
            </div>
            <div className = "oneGif">
              <p>Zobrazenie rozvhu predmetu</p>
              <img src={swapifyGIF} alt="zobrazenie rozvhu predmetu" />
            </div>
          </DialogContent>
        </Dialog>

        <Dialog
          fullWidth={true}
          maxWidth={'sm'}
          open={this.state.mailUsModalWindowOpen}
          onClose={() => this.setState({ mailUsModalWindowOpen: false })}
          onBackdropClick={() => this.setState({ mailUsModalWindowOpen: false })}
          aria-labelledby="max-width-dialog-title"
        >
          <DialogTitle id="max-width-dialog-title">Napíšte nám</DialogTitle>
          <DialogContent className = "dialogMailUsContent">
            <FormControl fullWidth>
              <TextField id="outlined-basic" label="Mailová adresa" fullWidth /> &nbsp;
              <TextField id="outlined-basic" label="Predmet" fullWidth /> &nbsp;
              <TextField id="outlined-multiline-flexible" label="Správa" fullWidth multiline rows="4"/> &nbsp;
              <Button>
                <SendIcon /> &nbsp; Odoslať
              </Button>
            </FormControl>
          </DialogContent>
        </Dialog>
        <BlockDetailContainer 
          user={this.state.user}
         />
      </div>
    );
  }
}


const mapStateToProps = state => ({ user: state.user });

export default connect(mapStateToProps)(TimetablePage);
