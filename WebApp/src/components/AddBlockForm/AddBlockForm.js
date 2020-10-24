import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../actions/blockDetailActions';
import { map, padStart, parseInt, replace } from 'lodash';
import styled from 'styled-components';
import { throttle } from "throttle-debounce";
import axios from 'axios';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import Radio from '@material-ui/core/Radio';
import RadioGroup from '@material-ui/core/RadioGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import InputLabel from '@material-ui/core/InputLabel';
import MenuItem from '@material-ui/core/MenuItem';
import FormControl from '@material-ui/core/FormControl';
import Select from '@material-ui/core/Select';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Autocomplete from './Autocomplete';
import * as timetableActions from '../../actions/timetableActions';
import { ClipLoader } from "react-spinners";

const FlexBox = styled.div`
  min-width: 400px;
  display: flex;
  flex-direction: column;
`;

class AddBlockForm extends Component {
  state = {
    id: this.props.course.id,
    courseName: this.props.course.courseName,
    courseShortcut: this.props.course.courseShortcut,
    teacher: this.props.course.teacher,
    room: this.props.course.room,
    day: this.props.course.day,
    startBlock: padStart(`${this.props.course.startBlock+6 || '07'}:00`, 5, '0'),
    length: (this.props.course.length == 2) ? 2 : this.props.course.endBlock - this.props.course.startBlock,
    type: this.props.course.type,
    suggestions: [],
    user: this.props.user,
    editing: this.props.editing,
    loading: false
  };

  handleCloseClick = () => this.props.onCloseEditBlock();
  
  handleSubmitClick = () => this.props.onSubmitClick();

  handleClickOutside = () =>  this.props.onCloseEditBlock();

  fetchCourses = () => {
    const fetch = throttle(500, courseName => {
      axios.get(`/api/timetable/course/getCoursesAutoComplete/${courseName}`).then(({ data }) => {
        this.setState({ suggestions: map(data, x => ({ ...x, label: x.courseName + ' ('+ x.courseCode +')'})) });
      });
    })
    return courseName => {
      if (courseName && courseName.length > 1) {
        fetch(courseName);
      }
    }
  }

  handleCourse = courseName => {
    this.setState({courseShortcut: courseName.split(' (').pop().split(')')[0]});
    this.setState({courseName: courseName.split(' (')[0]});
  } 

  handleChange = evt => {
    const { name, value } = evt.target;
    this.setState({ [name]: value });
  }

  canSubmit = () => {
    const { courseName, startBlock, length, type } = this.state;
    return courseName && startBlock && length && type;
  }

  submit = () => {
    this.setState({loading: true});
    const { onClose } = this.props;
    const { startBlock, length, ...restState } = this.state;
    const start = parseInt(replace(startBlock, /[^0-9]/, '')) / 100;
    const body = {
      user: this.props.user,
      timetableBlock: {
        ...restState,
        startBlock: start,
        endBlock: start + parseInt(length)
      }
    };
    this.handleSubmitClick();

    if (this.state.editing) {
      this.props.timetableActions.editBlock(body, this.state.user.email);
    } else {
      this.props.timetableActions.addBlock(body, this.props.user.email);
    }

    this.setState({loading: false});
    onClose();
  }


  render() {
    const { onClose } = this.props
    const { day, courseName, courseShortcut, teacher, room, startBlock, length, type, suggestions } = this.state;
    return (
      <form>
        <Dialog open onClose={evt => {
          evt.stopPropagation();
          onClose();
        }}>
            <div className="buttons">
        
        <IconButton onClick={this.handleCloseClick}>
          <ClearIcon nativecolor="black" />
        </IconButton>
      </div>
          <DialogContent>
            <FlexBox>
              <Autocomplete
                placeholder={courseName == "" ? "Zadajte názov predmetu *" : courseName + " (" + courseShortcut + ")"}
                name="courseName"
                value={courseName}
                suggestions={suggestions}
                inputvaluechange={this.fetchCourses()}
                onChange={this.handleCourse}
                margin="normal"
                fullWidth
                required
              />
              <TextField
                label="Profesor"
                placeholder="Meno profesora"
                name="teacher"
                value={teacher}
                onChange={this.handleChange}
                margin="normal"
                fullWidth
              />
              <TextField
                label="Miestnosť"
                placeholder="Miestnosť"
                name="room"
                value={room}
                onChange={this.handleChange}
                margin="normal"
                fullWidth
              />
              <FormControl fullWidth required>
                <InputLabel>Deň</InputLabel>
                <Select
                  name="day"
                  value={day}
                  onChange={this.handleChange}
                >
                  <MenuItem value={1}>Pondelok</MenuItem>
                  <MenuItem value={2}>Utorok</MenuItem>
                  <MenuItem value={3}>Streda</MenuItem>
                  <MenuItem value={4}>Štvrtok</MenuItem>
                  <MenuItem value={5}>Piatok</MenuItem>
                </Select>
              </FormControl>
              <TextField
                label="Začiatok"
                type="time"
                inputProps={{ step: 3600 }}
                name="startBlock"
                value={startBlock}
                onChange={this.handleChange}
                margin="normal"
                fullWidth
                required
              />
              <TextField
                label="Dĺžka"
                type="number"
                InputProps={{ inputProps: { min: 2, max: 4 } }}
                name="length"
                value={length}
                onChange={this.handleChange}
                margin="normal"
                fullWidth
                required
              />
              <RadioGroup
                name="type"
                value={type}
                onChange={this.handleChange}
              >
                <FormControlLabel label="Prednáška" value="lecture" control={<Radio />} />
                <FormControlLabel label="Laboratórium" value="laboratory" control={<Radio />} />
                <FormControlLabel label="Cvičenie" value="excercise" control={<Radio />} />
              </RadioGroup>
            </FlexBox>
          </DialogContent>
          <DialogActions>
          {!this.state.loading && 
            <Button
              disabled={!this.canSubmit()}
              onClick={this.submit}
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
          </DialogActions>
        </Dialog>
      </form>
    );
  }
}

AddBlockForm.defaultProps = {};

const mapStateToProps = (state) => ({
  ...state.timetable,
});

const mapDispatchToProps = (dispatch) => ({
  actions: bindActionCreators(actions, dispatch),
  timetableActions: bindActionCreators(timetableActions, dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(AddBlockForm);
