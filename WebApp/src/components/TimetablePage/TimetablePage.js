import React, { PureComponent } from 'react';
import Toolbar from '../Toolbar/Toolbar';
import Dialog from '@material-ui/core/Dialog';
import DialogTitle from '@material-ui/core/DialogTitle';
import DialogContent from '@material-ui/core/DialogContent';
import { AutoRotatingCarousel, Slide } from 'material-auto-rotating-carousel';
import { withStyles } from '@material-ui/core/styles';
import FormControl from '@material-ui/core/FormControl';
import TextField from '@material-ui/core/TextField';
import SendIcon from '@material-ui/icons/Send';
import { connect } from 'react-redux';
import TimetableContainer from '../../containers/TimetableContainer/TimetableContainer';
import BlockDetailContainer from '../../containers/BlockDetailContainer/BlockDetailContainer';
import SidebarContainer from '../Sidebar/SidebarContainer';
import { Button } from '@material-ui/core';

import GifAddCourse from '../../images/swapify-addCourse.gif';
import GifShowCourseTimetable from '../../images/swapify-showCourseTimetable.gif';
import GifAskForExchange from '../../images/swapify-askForExchange.gif';

import './TimetablePage.scss';

const { red, green, blue } = require('@material-ui/core/colors');

const carouselStyles = {
  content: {
    maxWidth: 1200,
    maxHeight: 800
  }
}

const StyledCarousel = withStyles(carouselStyles)(AutoRotatingCarousel);

class TimetablePage extends PureComponent {
  state = { 
    sidebarOpen: false, 
    helpModalWindowOpen: false,
    mailUsModalWindowOpen: false,
    user: this.props.user 
  };
  
  render() {
    return (
      <div className="app-container">
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
          toggleMailUsModalWindow={() =>
            this.setState(prevState => ({
              mailUsModalWindowOpen: !prevState.mailUsModalWindowOpen
            }))
          }
        />
        <SidebarContainer
          open={this.state.sidebarOpen}
          onClose={() => this.setState({ sidebarOpen: false })}
        />
        <TimetableContainer />

        <StyledCarousel
          label='Poďme na to!'
          open={this.state.helpModalWindowOpen}
          onClose={() => this.setState({ helpModalWindowOpen: false })}
          onStart={() => this.setState({ helpModalWindowOpen: false })}
          autoplay={false}
        >
          <Slide
            media={<img src={GifAddCourse} alt="pridanie bloku" />}
            title='Pridaj si blok predmetu'
            subtitle='Klikni na miesto v rozvrhu, kde má začínať blok. Napíš jeho názov, meno profesora, miestnosť. 
            Zvoľ či sa jedná o prednášku, cvičenie alebo laboratórium, poprípade zmeň jeho dĺžku a klikni na "ULOŽIŤ".'
            mediaBackgroundStyle={{ backgroundColor: red[400] }}
            style={{ backgroundColor: red[600] }}
          />  
          <Slide
            media={<img src={GifShowCourseTimetable} alt="zobrazenie rozvrhu predmetu" />}
            title='Zobraz si rozvrh predmetu'
            subtitle='Pozri sa ako vyzerá rozvrh predmetu v porovnaní s Tvojím vlastným. V bočnom menu si zvoľ predmety, 
            ktorých rozvrhy chceš vidieť v tom svojom.'
            mediaBackgroundStyle={{ backgroundColor: green[400] }}
            style={{ backgroundColor: green[600] }}
          />
          <Slide
            media={<img src={GifAskForExchange} alt="poziadanie o vymenu cvicenia" />}
            title='Požiadaj o výmenu cvičenia'
            subtitle='Vyber si cvičenie, ktoré si chceš vymeniť, klikni naň a zvoľ "Požiadať o výmenu". Následne si vyber 
            zo zobrazených cvičení to, ktoré ti najviac vyhovuje. Potom už len čakaj kým, si niekto bude chcieť vymeniť 
            dané cvičenie tiež.'
            mediaBackgroundStyle={{ backgroundColor: blue[400] }}
            style={{ backgroundColor: blue[600] }}
          />
        </StyledCarousel>
        
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
