import React from 'react';
import Toolbar from '../Toolbar/Toolbar';
import { connect } from 'react-redux';
import Button from '@material-ui/core/Button';
import NavigationIcon from '@material-ui/icons/Navigation';
import FormControl from '@material-ui/core/FormControl';
import TextField from '@material-ui/core/TextField';
import './StudyGroup.scss';
import axios from 'axios';
import { MacBackground } from '../';
import { TIMETABLE } from '../../util/routes';

class StudyGroup extends React.Component {
  state = {
    group: ''
  }

  Submit = () => {
    const body = {
      groupNumber: this.state.group
    }
    axios({
      method: 'post',
      url: '/api/timetable',
      data: body
    })
      .then(() => {
        this.props.history.push(TIMETABLE);
      });
  }

  handleSubmit = (evt) => {
    this.setState({ group: evt.target.value });
  }

  canBeSubmitted = () => {
    return this.state.group.length === 6;
  }

  render() {
    return (
        <MacBackground>
            <div className="container home">
                 <Toolbar />
                <div className="StudyGroup-wrapper">
                <FormControl
                    fullWidth
                >
                    <TextField
                      error={this.state.group.length !== 6 && this.state.group !== ""}
                      id="group"
                      value={this.state.group}
                      onChange={this.handleSubmit}
                      label="Zadajte štud. skupinu"
                      placeholder="Príklad 5ZZS12"
                      margin="normal"
                      fullWidth
                      multiline
                      helperText={this.state.group.length !== 6 && this.state.group !== "" ? 'Zlý format štud. skupiny' : ' '}
                    />
                </FormControl>
                <Button
                    color="primary"
                    variant="fab"
                    mini
                    className="fab"
                    onClick={this.Submit}
                    disabled={!this.canBeSubmitted()}
                >
                    <NavigationIcon />
                </Button>
                </div>
            </div>
      </MacBackground>
    );
  }
}

const mapStateToProps = state => ({ user: state.user });

export default connect(mapStateToProps)(StudyGroup);