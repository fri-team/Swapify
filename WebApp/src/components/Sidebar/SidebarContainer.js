import React, { PureComponent } from 'react';
import _ from 'lodash';
import { connect } from 'react-redux';
import {
  showCourseTimetable,
  hideCourseTimetable
} from '../../actions/timetableActions';

import {
  loadExchangeRequests
} from '../../actions/exchangeActions'

import Sidebar from './Sidebar';

class SidebarContainer extends PureComponent {
  state = {
    value: 0,
  };

  componentDidMount() {
    this.props.loadWaitingExchangeRequests();  
  }

  handleChange = (event, value) => {
    this.setState({ value });
  };

  handleCourseToggle = (courseId, courseName, checked) => {
    const { dispatch } = this.props;
    if (checked) {
      dispatch(showCourseTimetable(courseId, courseName));
    } else {
      dispatch(hideCourseTimetable(courseId));
    }
  };

  render() {
    const { open, onClose, myCourseNames, displayedCourses, exchangeRequests } = this.props;
    const { value } = this.state;
    const courses = _.map(myCourseNames, course => ({
      courseName: course.courseName,
      courseId: course.courseId,
      checked: _.includes(displayedCourses, course.courseId)
    }));
    return (
      <Sidebar
        open={open}
        onClose={onClose}
        courses={courses}
        onCourseToggle={this.handleCourseToggle}
        handleChange={this.handleChange}
        value={value}
        exchangeRequests={exchangeRequests}        
      />
    );
  }
}

const mapStateToProps = state => ({ ...state.timetable, ...state.exchangeRequests });
const mapDispatchToProps = dispatch => {
  return {
    loadWaitingExchangeRequests: () => dispatch(loadExchangeRequests()) 
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(SidebarContainer);