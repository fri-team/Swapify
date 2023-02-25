import React, { Component } from 'react';
import styled from 'styled-components';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import IconButton from '@material-ui/core/IconButton';
import ClearIcon from '@material-ui/icons/Clear';
import Autocomplete from '../AddBlockForm/Autocomplete';
import { ClipLoader } from "react-spinners";
import { throttle } from "throttle-debounce";
import axios from 'axios';
import { map } from 'lodash';
import { connect } from 'react-redux';
import {
  showCourseTimetable, hideCourseTimetable, loadMyTimetable
}  from '../../actions/timetableActions';

const FlexBox = styled.div`
  min-width: 400px;
  min-height: 300px;
  display: flex;
  flex-direction: column;
`;

class SideBarForm extends Component {
    state = {
        courseId: '',
        courseInfo:[],
        courseName: '',
        courseShortcut: '',
        yearOfStudy: '',
        studyType: '',
        suggestions: [],
        loading: false
    };

    fetchCourses = () => {
        const fetch = throttle(500, courseName => {
          axios
            .get(
              `/api/timetable/course/getCoursesAutoComplete/${courseName}/${this.props.user.timetableId}`
            )
            .then(({ data }) => {
              this.setState({
                suggestions: map(data, (x) => ({
                  ...x,
                  label:
                    x.courseName +
                    " (" +
                    x.courseCode +
                    ") " +
                    x.yearOfStudy +
                    ".r," +
                    this.cutStudyType(x.studyType),
                })),
              });
              this.setState({
                courseInfo: map(data, (x) => ({
                  ...x,
                  label:
                    x.courseName +
                    " (" +
                    x.courseCode +
                    ") " +
                    x.id +
                    "," +
                    x.yearOfStudy +
                    "," +
                    this.cutStudyType(x.studyType),
                })),
              });
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

    canSubmit = () => {
        const { courseName } = this.state;
        return courseName != '' ? true : false;
    }

    submit = () => {
      const { courseId, courseName } = this.state;
      const { onClose } = this.props;

      this.props.showCourseTimetable(courseId, courseName);

      this.setState({loading: false});
      onClose();
    }

    handleCourse = (courseName) => {
      this.setState({courseShortcut: courseName.split(' (').pop().split(') ')[0]});
      this.setState({courseName: courseName.split(' (')[0]});

      for(var i = 0; i < this.state.courseInfo.length; i++){
        var shortCut = this.state.courseInfo[i].label.split(' (').pop().split(') ')[0];
        var name = this.state.courseInfo[i].label.split(' (')[0];
        var year = this.state.courseInfo[i].label.split(',')[1];
        var type = this.state.courseInfo[i].label.split(',')[2];

        if(courseName.split(' (')[0] === name && courseName.split(' (').pop().split(') ')[0] === shortCut
          && courseName.split(') ')[1].split(',')[0] === year+'.r' && courseName.split(') ')[1].split(',')[1] === type) {
          this.setState({courseId: this.state.courseInfo[i].label.split(') ')[1].split(',')[0]});
        }
      }
    }

    render() {
        const { onClose } = this.props
        const { courseName,courseShortcut, yearOfStudy, studyType, suggestions } = this.state;
        return (
          <form>
            <Dialog open onClose={evt => {
              evt.stopPropagation();
              onClose();
            }}>
              <div className="buttons">
                <IconButton onClick={onClose}>
                  <ClearIcon nativecolor="black" />
                </IconButton>
              </div>
              <DialogContent>
                  <FlexBox>
                  <Autocomplete
                    placeholder={courseName == "" ? "Zadajte názov predmetu *" : courseName + " (" + courseShortcut + ") " + yearOfStudy + ".r," + studyType}
                    name="courseName"
                    value={courseName}
                    suggestions={suggestions}
                    inputvaluechange={this.fetchCourses()}
                    onChange={this.handleCourse}
                    margin="normal"
                    fullWidth
                    required
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
              Vyhľadať
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

SideBarForm.defaultProps = {};

const mapStateToProps = (state) => ({
  ...state.timetable,
  user: state.user,
});

const mapDispatchToProps = (dispatch) => ({
  showCourseTimetable: (courseId, courseName) => dispatch(showCourseTimetable(courseId, courseName)),
  hideCourseTimetable: (courseId) => dispatch(hideCourseTimetable(courseId)),
  loadMyTimetable: (user, history) => dispatch(loadMyTimetable(user, history))
});

export default connect(mapStateToProps, mapDispatchToProps)(SideBarForm);
