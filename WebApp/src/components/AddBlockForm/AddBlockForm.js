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
import { SliderPicker } from 'react-color';
import toMaterialStyle from 'material-color-hash';

const FlexBox = styled.div`
  min-width: 400px;
  display: flex;
  flex-direction: column;
`;

class AddBlockForm extends Component {
  state = {
    id: this.props.course.id,
    courseName: this.props.course.courseName,
    courseCode: this.props.course.courseCode,
    courseShortcut: this.props.course.courseShortcut,
    teacher: this.props.course.teacher,
    room: this.props.course.room,
    day: this.props.course.day,
    startBlock: padStart(`${this.props.course.startBlock+6 || '07'}:00`, 5, '0'),
    length: this.props.course.endBlock - this.props.course.startBlock,
    type: this.props.course.type,
    suggestions: [],
    user: this.props.user,
    editing: this.props.editing,
    loading: false,
    blockColor: this.props.course.blockColor == null ?
      toMaterialStyle(this.props.course.courseCode, this.props.course.blockColor).backgroundColor : this.setBlockColor(this.props.course.blockColor).backgroundColor,
    yearOfStudy: '',
    studyType: '',
  };

  handleCloseClick = () => this.props.onCloseEditBlock();

  handleSubmitClick = () => this.props.onSubmitClick();

  handleClickOutside = () =>  this.props.onCloseEditBlock();

  fetchCourses = () => {
    const fetch = throttle(500, courseName => {
      axios.get(`/api/timetable/course/getCoursesAutoComplete/${courseName}/${this.state.user.timetableId}`).then(({ data }) => {
        this.setState({ suggestions: map(data, x => ({ ...x, label: x.courseName + ' ('+ x.courseCode +') '+ x.yearOfStudy + ".r," + this.cutStudyType(x.studyType)})) });
      });
    })

    return courseName => {
      if (courseName && courseName.length > 1) {
        fetch(courseName);
      }
    }
  }

  cutStudyType = (studyType) => {
    var array = studyType.split(' ');
    var returnedString = '';
    for (var i = 0; i < 2; i++) {
      returnedString += array[i].substring(0,3) + '.'
    }
    return returnedString;
  }

  fetchCourseBlock = courseName => {
    this.setState({courseCode: courseName.split(' (').pop().split(') ')[0]});
    this.setState({courseName: courseName.split(' (')[0]});

    const startBlock = parseInt(this.state.startBlock.split(':')[0]);

    var j = 0;
    for (var i = 0; i < this.state.suggestions.length; i++) {
      if (this.state.suggestions[i].courseName == courseName.split(' (')[0] && this.state.suggestions[i].courseCode == courseName.split(' (')[1].split(') ')[0] &&
          this.state.suggestions[i].yearOfStudy + '.r' == courseName.split(') ')[1].split(',')[0] && this.cutStudyType(this.state.suggestions[i].studyType) == courseName.split(') ')[1].split(',')[1]) {
        j = i;
      }
    }

    axios.get(`/api/timetable/getCourseBlock/${this.state.suggestions[j].id}/${startBlock}/${this.state.day}`).then(({ data }) => {
      this.setState({ teacher: data.teacher });
      this.setState({ room: data.room });
      this.setState({ length: data.endBlock - data.startBlock });
      this.setState({ type: data.type });
    }).catch(function (error) {
      if (error.response.status == '404') {
        alert('Upozornenie: zadaný predmet sa v tomto čase nevyučuje');
      }
    });

  }

  handleChange = evt => {
    const { name, value } = evt.target;
    if (name == "length") {
      if (this.state.startBlock.endsWith("PM")) {
        let maxLength = 8 - this.state.startBlock.substring(0, 2);
        if (value > maxLength) {
          this.setState({ [name]: maxLength });
        } else if (value < 1) {
          this.setState({ [name]: 1 });
        } else {
          this.setState({ [name]: value });
        }
      } else {
        let maxLength = 20 - this.state.startBlock.substring(0, 2);
        if (value > maxLength) {
          this.setState({ [name]: maxLength });
        } else if (value < 1) {
          this.setState({ [name]: 1 });
        } else {
          this.setState({ [name]: value });
        }
      }
    } else if (name == "startBlock") {
      if (this.state.startBlock.endsWith("AM")) {
        if (value.substring(0,2) < "07") {
          this.setState({ [name]: "07:00 AM" });
        } else if (value.substring(0,2) > "11") {
          this.setState({ [name]: "11:00 AM" });
          if (this.state.length > 9) {
            this.setState({ length: 9 });
          }
        } else {
          let maxLength = 20 - value.substring(0,2);
          if (this.state.length > maxLength) {
            this.setState({ length: maxLength });
          }
          this.setState({ [name]: value });
        }
      } else if (this.state.startBlock.endsWith("PM")) {
        if (value.substring(0,2) < "01") {
          this.setState({ [name]: "01:00 PM" });
        } else if (value.substring(0,2) == "12") {
          this.setState({ [name]: "12:00 PM" });
          if (this.state.length > 8) {
            this.setState({ length: 8 });
          }
        } else if (value.substring(0,2) > "07") {
          this.setState({ [name]: "07:00 PM", length: 1 });
        } else {
          let maxLength = 8 - value.substring(0,2);
          if (this.state.length > maxLength) {
            this.setState({ length: maxLength });
          }
          this.setState({ [name]: value });
        }
      } else {
        if (value.substring(0,2) < "07") {
          this.setState({ [name]: "07:00" });
        } else if (value.substring(0,2) > "19") {
          let val = "19";
          if (this.state.length > (20 - val)) {
            this.setState({ length: (20 - val) });
          }
          this.setState({ [name]: "19:00" });
        } else {
          if (this.state.length > (20 - value.substring(0,2))) {
            this.setState({ length: (20 - value.substring(0,2)) });
          }
          this.setState({ [name]: value.substring(0,3) + "00" });
        }
      }
    } else if (name == "colorOfBlock") {
      this.setState({blockColor: value})
    } else {
      this.setState({ [name]: value });
    }
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

  handleChangeComplete = (color) => {
    this.setState({blockColor: color.hex});
  }

  setBlockColor(shade) {
    return {
      backgroundColor: shade,
      color: 'rgba(0, 0, 0, 0.87)',
    };
  }

  render() {
    const { onClose } = this.props
    const { day, courseName, courseCode: courseCode, teacher, room, startBlock, length, type, suggestions } = this.state;
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
                placeholder={courseName == "" ? "Zadajte názov predmetu *" : courseName + " (" + courseCode + ") "}
                name="courseName"
                value={courseName}
                suggestions={suggestions}
                inputvaluechange={this.fetchCourses()}
                onChange={this.fetchCourseBlock}
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
                InputProps={{ inputProps: { min: 1, max: (20 - this.state.startBlock.substring(0,2)) } }}
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

              <TextField
                 label="Farba bloku"
                 type="text"
                 inputProps={{ step: 3600 }}
                 name="colorOfBlock"
                 value={this.state.blockColor}
                 onChange={this.handleChange}
                 margin="normal"
                 fullWidth
                 required
              />
              <SliderPicker
                color={ this.state.blockColor }
                onChangeComplete={this.handleChangeComplete}
              />
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
