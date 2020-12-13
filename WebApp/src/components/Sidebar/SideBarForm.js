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
  showCourseTimetable
} from '../../actions/timetableActions';
//import * as timetableActions from '../../actions/timetableActions';

//import { bindActionCreators } from 'redux';
//import * as actions from '../../actions/blockDetailActions';


const FlexBox = styled.div`
  min-width: 400px;
  min-height: 300px;
  display: flex;
  flex-direction: column;
`;

class SideBarForm extends Component {
    state = {
        courseId: '',
        pole:[],
        courseName: '',
        courseShortcut: '',
        suggestions: [],
        loading: false
    };

    fetchCourses = () => {
        const fetch = throttle(500, courseName => {
          axios.get(`/api/timetable/course/getCoursesAutoComplete/${courseName}`).then(({ data }) => {
            this.setState({ suggestions: map(data, x => ({ ...x, label: x.courseName + ' ('+ x.courseCode +')' + x.id})) });
          });
        })
        return courseName => {
          if (courseName && courseName.length > 1) {
            fetch(courseName);
          }
        }
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
      this.setState({courseShortcut: courseName.split(' (').pop().split(')')[0]});
      this.setState({courseName: courseName.split(' (')[0]});
      this.setState({courseId: courseName.split(')')[1]});
    } 

    render() {
        const { onClose } = this.props
        const { courseName,courseShortcut, suggestions } = this.state;
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
});

const mapDispatchToProps = (dispatch) => ({
  //actions: bindActionCreators(actions, dispatch),
  //timetableActions: bindActionCreators(timetableActions, dispatch),
  showCourseTimetable: (courseId, courseName) => dispatch(showCourseTimetable(courseId, courseName)),
});

export default connect(mapStateToProps, mapDispatchToProps)(SideBarForm);
                 