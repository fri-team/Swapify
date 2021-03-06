import React from 'react';
import Toolbar from '../Toolbar/Toolbar';
import { connect } from 'react-redux';
import Button from '@material-ui/core/Button';
import FormControl from '@material-ui/core/FormControl';
import TextField from '@material-ui/core/TextField';
import './PersonalNumber.scss';
import axios from 'axios';
import { MacBackground } from '..';
import { TIMETABLE } from '../../util/routes';
import { ClipLoader } from "react-spinners";


class PersonalNumber extends React.Component {
  state = {
    loading: false,
    personalNumber: '',
    user: this.props.user,
    existing: false,
    
  }


  onKeyDown = (event) => {
    if (event.key === 'Enter') {
      event.preventDefault();
      this.Submit();
    }
  }


  Submit = () => {
    this.setState({loading: true});
    const body = {
      personalNumber: this.state.personalNumber,
      email: this.state.user.email
    }
    axios({
      method: 'post',
      url: '/api/timetable/setStudentTimetableFromPersonalNumber',
      data: body
    })
    .then(() => {
      this.setState({loading: false});
      this.props.history.push(TIMETABLE);
    })
    .catch(() => {
      this.setState({loading: false});
      this.setState({existing: true});
    })
  }

  handleSubmit = (evt) => {
    this.setState({ personalNumber: evt.target.value });
    this.setState({existing: false});
  }

  canBeSubmitted = () => {
    return this.state.personalNumber.length === 6;
  }

  render() {
    return (
        <MacBackground>
            <div className="app-container home">
                 <Toolbar />
                <div className="PersonalNumber-wrapper">
                <FormControl
                    fullWidth
                >
                    <TextField
                      error={(this.state.personalNumber.length !== 6 && this.state.personalNumber !== "") || this.state.existing}
                      id="personalNumber"
                      value={this.state.personalNumber}
                      onChange={this.handleSubmit}
                      label="Zadajte osobné číslo"
                      placeholder="Príklad 555000"
                      margin="normal"
                      fullWidth
                      multiline
                      autoFocus
                      onKeyDown={this.onKeyDown}
                      helperText={this.state.personalNumber.length !== 6 && this.state.personalNumber !== "" ? 'Zlý formát osobného čísla' : this.state.existing ? 'Neexistujúce osobné číslo' : ''}
                    />
                </FormControl>
                
                {!this.state.loading && 
                  <Button 
                      onClick={this.Submit}
                      disabled={!this.canBeSubmitted()}
                      color="primary" 
                      variant="contained"
                  >
                    Uložiť 
                  </Button>
                }
                {this.state.loading && 
                    <ClipLoader
                      size={35}
                      color={"#2196f3"}
                      loading={this.state.loading}
                    />
                }
                </div>
            </div>
      </MacBackground>
    );
  }
}

const mapStateToProps = state => ({ user: state.user });

export default connect(mapStateToProps)(PersonalNumber);
