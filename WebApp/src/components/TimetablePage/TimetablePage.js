import React, { PureComponent } from "react";
import Toolbar from "../Toolbar/Toolbar";
import Dialog from "@material-ui/core/Dialog";
import DialogTitle from "@material-ui/core/DialogTitle";
import DialogContent from "@material-ui/core/DialogContent";
import { AutoRotatingCarousel, Slide } from "material-auto-rotating-carousel";
import { withStyles } from "@material-ui/core/styles";
import FormControl from "@material-ui/core/FormControl";
import TextField from "@material-ui/core/TextField";
import SendIcon from "@material-ui/icons/Send";
import { connect } from "react-redux";
import TimetableContainer from "../../containers/TimetableContainer/TimetableContainer";
import BlockDetailContainer from "../../containers/BlockDetailContainer/BlockDetailContainer";
import SidebarContainer from "../Sidebar/SidebarContainer";
import { Button } from "@material-ui/core";
import { messageChanged, sendFeedback, setBlockedHours } from "../../actions/toolbarActions";
import axios from "axios";
import * as actions from '../../actions/timetableActions';

import GifAddCourse from "../../images/swapify-addCourse.gif";
import GifShowCourseTimetable from "../../images/swapify-showCourseTimetable.gif";
import GifAskForExchange from "../../images/swapify-askForExchange.gif";

import "./TimetablePage.scss";

const { red, green, blue, blueGrey } = require("@material-ui/core/colors");

const carouselStyles = {
  content: {
    maxWidth: 1200,
    maxHeight: 800,
  },
};

const StyledCarousel = withStyles(carouselStyles)(AutoRotatingCarousel);

class TimetablePage extends PureComponent {
  constructor(props) {
    super();

    this.state = {
      sidebarOpen: false,
      helpModalWindowOpen: false,
      mailUsModalWindowOpen: false,
      changeDarkMode: false,
      user: props.user,
      message: "",
      subject: "",
      darkMode: true,
      updateBlockedHoursVisibility: false,
      timetableType: 3 // 3 = unknown
    };
    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.getDarkMode();
    this.getBlockedHoursVisibility();
    this.getTimetableType();

  }

  handleChange(e) {
    let target = e.target;
    let value = target.value;
    let name = target.name;

    this.setState({
      [name]: value,
    });
  }

  handleSubmit(e) {
    e.preventDefault();

    this.props.sendFeedback(
      this.props.user.email,
      this.props.user.name + " " + this.props.user.surname,
      this.state.subject,
      this.state.message
    );

    this.setState({
      mailUsModalWindowOpen: false,
      message: "",
      subject: ""
    });
  }

  getDarkMode() {
    const body = {
      email: this.state.user.email,
      darkMode: "true"
    };
    axios({
      method: "post",
      url: "/api/user/getDarkMode",
      data: body
    }).then(response => {
      this.setState({ darkMode: response.data });
    });
  }

  swapDarkMode() {
    var dark = !this.state.darkMode;
    this.setState({ darkMode: dark });

    const body = {
      email: this.state.user.email,
      darkMode: dark
    };

    axios({
      method: "post",
      url: "/api/user/setDarkMode",
      data: body
    }).then();
  }

  getBlockedHoursVisibility() {
    const body = {
      email: this.state.user.email,
      blockedHours: false
    };

    axios({
      method: "post",
      url: "/api/user/getBlockedHoursVisibility",
      data: body
    }).then(response => {
      this.props.setBlockedHours(response.data);
    });
  }

  changeBlockedHourVisibility() {
    const body = {
      email: this.state.user.email,
      blockedHours: !this.props.showBlockedHours
    };
    axios({
      method: "post",
      url: "/api/user/setBlockedHoursVisibility",
      data: body
    }).then(response => {
      this.props.setBlockedHours(response.data);
    });
  }

  getTimetableType() {
    const body = {
      email: this.state.user.email
    };

    axios({
      method: "post",
      url: "/api/timetable/getTimetableType",
      data: body
    }).then(response => {
      this.setState({ timetableType: response.data });
    });
  }

  render() {
    return (
      <div className="app-container">
        <Toolbar
          timetableType={this.state.timetableType}
          darkMode={this.state.darkMode}
          changeDarkMode={() =>
            this.swapDarkMode()
          }
          updateBlockedHoursVisibility={() =>
            this.changeBlockedHourVisibility()
          }
          toggleSidebar={() =>
            this.setState((prevState) => ({
              sidebarOpen: !prevState.sidebarOpen,
            }))
          }
          exportCalendar={() => {
            // not exactly sure how to work with these actions so I imported it like this
            actions.loadMyTimetableCalendar(this.props.user, this.props.history).then(res => {
              var uri = "data:text/calendar;charset=utf8," + res;

              var downloadLink = document.createElement("a");
              downloadLink.href = uri;
              downloadLink.download = "timetable.ics";

              document.body.appendChild(downloadLink);
              downloadLink.click();
              document.body.removeChild(downloadLink);
            })
          }
          }
          toggleHelpModalWindow={() =>
            this.setState((prevState) => ({
              helpModalWindowOpen: !prevState.helpModalWindowOpen,
            }))
          }
          toggleMailUsModalWindow={() =>
            this.setState((prevState) => ({
              mailUsModalWindowOpen: !prevState.mailUsModalWindowOpen,
            }))
          }
        />
        <SidebarContainer
          timetableType={this.state.timetableType}
          darkMode={this.state.darkMode}
          open={this.state.sidebarOpen}
          onClose={() => this.setState({ sidebarOpen: false })}
        />
        <TimetableContainer
          darkMode={this.state.darkMode}
          history={this.props.history}
        />

        <StyledCarousel
          label="Poďme na to!"
          open={this.state.helpModalWindowOpen}
          onClose={() => this.setState({ helpModalWindowOpen: false })}
          onStart={() => this.setState({ helpModalWindowOpen: false })}
          autoplay={false}
        >
          <Slide
            media={<img src={GifAddCourse} alt="pridanie bloku" />}
            title="Pridaj si blok predmetu"
            subtitle='Klikni na miesto v rozvrhu, kde má začínať blok. Napíš jeho názov, meno profesora, miestnosť.
            Zvoľ či sa jedná o prednášku, cvičenie alebo laboratórium, poprípade zmeň jeho dĺžku a klikni na "ULOŽIŤ".'
            mediaBackgroundStyle={{ backgroundColor: this.state.darkMode ? blueGrey[400] : red[400] }}
            style={{ backgroundColor: this.state.darkMode ? blueGrey[600] : red[600] }}
          />
          <Slide
            media={
              <img
                src={GifShowCourseTimetable}
                alt="zobrazenie rozvrhu predmetu"
              />
            }
            title="Zobraz si rozvrh predmetu"
            subtitle="Pozri sa ako vyzerá rozvrh predmetu v porovnaní s Tvojím vlastným. V bočnom menu si zvoľ predmety,
            ktorých rozvrhy chceš vidieť v tom svojom."
            mediaBackgroundStyle={{ backgroundColor: this.state.darkMode ? blueGrey[400] : green[400] }}
            style={{ backgroundColor: this.state.darkMode ? blueGrey[600] : green[600] }}
          />
          <Slide
            media={
              <img src={GifAskForExchange} alt="poziadanie o vymenu cvicenia" />
            }
            title="Požiadaj o výmenu cvičenia"
            subtitle='Vyber si cvičenie, ktoré si chceš vymeniť, klikni naň a zvoľ "Požiadať o výmenu". Následne si vyber
            zo zobrazených cvičení to, ktoré ti najviac vyhovuje. Potom už len čakaj kým, si niekto bude chcieť vymeniť
            dané cvičenie tiež.'
            mediaBackgroundStyle={{ backgroundColor: this.state.darkMode ? blueGrey[400] : blue[400] }}
            style={{ backgroundColor: this.state.darkMode ? blueGrey[600] : blue[600] }}
          />
        </StyledCarousel>

        <Dialog
          fullWidth={true}
          maxWidth={"sm"}
          open={this.state.mailUsModalWindowOpen}
          onClose={() => this.setState({ mailUsModalWindowOpen: false })}
          onBackdropClick={() =>
            this.setState({ mailUsModalWindowOpen: false })
          }
          aria-labelledby="max-width-dialog-title"
        >
          <DialogTitle id="max-width-dialog-title"
            style={{ backgroundColor: this.state.darkMode ? "#808080" : "white" }}>Napíšte nám</DialogTitle>
          <DialogContent className="dialogMailUsContent"
            style={{ backgroundColor: this.state.darkMode ? "#808080" : "white" }}>
            <FormControl fullWidth>
              <TextField
                id="outlined-basic"
                label="Predmet"
                name="subject"
                value={this.state.subject}
                onChange={this.handleChange}
                fullWidth
              />{" "}
              &nbsp;
              <TextField
                id="outlined-multiline-flexible"
                label="Správa"
                name="message"
                fullWidth
                multiline
                rows="4"
                value={this.state.message}
                onChange={this.handleChange}
              />{" "}
              &nbsp;
              <Button onClick={this.handleSubmit}>
                <SendIcon /> &nbsp; Odoslať
              </Button>
            </FormControl>
          </DialogContent>
        </Dialog>
        <BlockDetailContainer user={this.state.user} />
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  user: state.user,
  message: state.message,
  showBlockedHours: state.toolbar.showBlockedHours
});

export default connect(mapStateToProps, { messageChanged, sendFeedback, setBlockedHours })(
  TimetablePage
);
