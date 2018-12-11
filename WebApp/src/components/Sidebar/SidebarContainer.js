import React, { PureComponent } from 'react';
import _ from 'lodash';
import { connect } from 'react-redux';
import {
  showCourseTimetable,
  hideCourseTimetable
} from '../../actions/timetableActions';
import Sidebar from './Sidebar';

class SidebarContainer extends PureComponent {
  state = {
    value: 0,
  };

  handleChange = (event, value) => {
    this.setState({ value });
  };

  handleCourseToggle = (course, checked) => {
    const { dispatch } = this.props;
    if (checked) {
      dispatch(showCourseTimetable(course));
    } else {
      dispatch(hideCourseTimetable(course));
    }
  };

  render() {
    const { open, onClose, myCourseNames, displayedCourses } = this.props;
    const { value } = this.state;
    const courses = _.map(myCourseNames, course => ({
      name: course,
      checked: _.includes(displayedCourses, course)
    }));
    return (
      <Sidebar
        open={open}
        onClose={onClose}
        courses={courses}
        onCourseToggle={this.handleCourseToggle}
        handleChange={this.handleChange}
        value={value}
      />
    );
  }
}

const mapStateToProps = state => ({ ...state.timetable });

export default connect(mapStateToProps)(SidebarContainer);