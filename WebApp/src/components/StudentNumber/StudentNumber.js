import React from 'react';
import Toolbar from '../Toolbar/Toolbar';
import { connect } from 'react-redux';
import Button from '@material-ui/core/Button';
import FormControl from '@material-ui/core/FormControl';
import TextField from '@material-ui/core/TextField';
import './StudentNumber.scss';
import axios from 'axios';
import { MacBackground } from '..';
import { TIMETABLE } from '../../util/routes';

class StudentNumber extends React.Component {
  state = {
    group: '',
    user: this.props.user,
    existing: false
  }

  onKeyDown = (event) => {
    if (event.key === 'Enter') {
      event.preventDefault();
      this.Submit();
    }
  }

  Submit = () => {
    const body = {
      studentNumber: this.state.group,
      email: this.state.user.email
    }
    axios({
      method: 'post',
      url: '/api/timetable/setStudentTimetableFromStudentNumber',
      data: body
    })
    .then(() => {
      this.props.history.push(TIMETABLE);
    })
    .catch(() => {
      this.setState({existing: true});
    })
  }

  handleSubmit = (evt) => {
    this.setState({ group: evt.target.value });
    this.setState({existing: false});
  }

  canBeSubmitted = () => {
    return this.state.group.length === 6;
  }

  render() {
    return (
        <MacBackground>
            <div className="container home">
                 <Toolbar />
                <div className="StudentNumber-wrapper">
                <FormControl
                    fullWidth
                >
                    <TextField
                      error={(this.state.group.length !== 6 && this.state.group !== "") || this.state.existing}
                      id="group"
                      value={this.state.group}
                      onChange={this.handleSubmit}
                      label="Zadajte osobné čislo"
                      placeholder="Príklad 555000"
                      margin="normal"
                      fullWidth
                      multiline
                      autoFocus
                      onKeyDown={this.onKeyDown}
                      helperText={this.state.group.length !== 6 && this.state.group !== "" ? 'Zlý formát osobného čísla' : this.state.existing ? 'Neexistujúce osobné číslo' : ''}
                    />
                </FormControl>
                <Button 
                    onClick={this.Submit}
                    disabled={!this.canBeSubmitted()}
                    color="primary" 
                    variant="contained"
                >
                  Uloziť
                </Button>
                </div>
            </div>
      </MacBackground>
    );
  }
}

const mapStateToProps = state => ({ user: state.user });

export default connect(mapStateToProps)(StudentNumber);