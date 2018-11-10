import React, { PureComponent } from 'react';
import { includes, map } from 'lodash';
import { connect } from 'react-redux';
import {
  showCourseTimetable,
  hideCourseTimetable
} from '../../actions/timetableActions';
import Sidebar from './Sidebar';

class SidebarContainer extends PureComponent {
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
    const courses = map(myCourseNames, course => ({
      name: course,
      checked: includes(displayedCourses, course)
    }));
    return (
      <Sidebar
        open={open}
        onClose={onClose}
        courses={courses}
        onCourseToggle={this.handleCourseToggle}
      />
    );
  }
}

const mapStateToProps = state => ({ ...state.timetable });

export default connect(mapStateToProps)(SidebarContainer);
