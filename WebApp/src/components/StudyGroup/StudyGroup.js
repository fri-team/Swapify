import React from 'react';
import Toolbar from '../Toolbar/Toolbar';
import { connect } from 'react-redux';
import Button from '@material-ui/core/Button';
import NavigationIcon from '@material-ui/icons/Navigation';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import Input from '@material-ui/core/Input';
import './StudyGroup.scss';
import axios from 'axios';
import { MacBackground } from '../';
import { TIMETABLE } from '../../util/routes';

class StudyGroup extends React.Component {
  state = {
    group: ''
  }

  Submit = () => {
    const data = {
      studyGroupNumber: this.state.group
    }
    axios({
      method: 'post',
      url: '/api/timetable/SetStudentTimetableFromGroup',
      data
    })
      .then(() => {
        this.props.history.push(TIMETABLE);
      });
  }

handleSubmit = (evt) => {
    this.setState({ group: evt.target.value });
  }

  canBeSubmitted = () => {
    return Boolean(this.state.group);

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
                    <InputLabel htmlFor="group">Vyberte štud. skupinu</InputLabel>
                    <Select
                    value={this.state.group}
                    onChange={this.handleSubmit}
                    input={<Input name="group" id="group" />}
                    autoWidth
                    >
                    <MenuItem value="">
                        <em>Nevybratá štud. skupina</em>
                    </MenuItem>
                    <MenuItem value='5ZZS11'>5ZZS11</MenuItem>
                    <MenuItem value='5ZZS12'>5ZZS12</MenuItem>
                    <MenuItem value='5ZZS13'>5ZZS13</MenuItem>
                    <MenuItem value='5ZZS21'>5ZZS21</MenuItem>
                    <MenuItem value='5ZZS22'>5ZZS22</MenuItem>
                    <MenuItem value='5ZZS23'>5ZZS23</MenuItem>
                    </Select>
                </FormControl>
                <br />
                <br />
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