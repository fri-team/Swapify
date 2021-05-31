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
    sideBarFormOpen: false,
  };

  componentDidMount() {
    this.props.loadWaitingExchangeRequests();  
  }

  handleChange = (event, value) => {
    this.setState({ value });
  };

  handleCourseToggle = (courseId, courseName, checked) => {
    if (checked) {
      this.props.showCourseTimetable(courseId, courseName);
    } else {
      this.props.hideCourseTimetable(courseId);
    }
  };

  handleAddClick = () => {
    if(!this.state.sideBarFormOpen) {
      this.setState({ sideBarFormOpen: true});
    } else {
      this.setState({ sideBarFormOpen: false});
    }
  };

  handleClickOutsideSideBarForm = () => {
    this.setState({sideBarFormOpen: false});
  }

  render() {
    const { open, onClose, myCourseNames, displayedCourses, exchangeRequests } = this.props;
    const { value } = this.state;
    const { sideBarFormOpen } = this.state;
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
        addClickHandle={this.handleAddClick}
        sideBarFormOpen={sideBarFormOpen}
        onCloseForm={this.handleClickOutsideSideBarForm}     
        darkMode={this.props.darkMode}
      />
    );
  }
}

const mapStateToProps = state => ({ ...state.timetable, ...state.exchangeRequests });
const mapDispatchToProps = dispatch => {
  return {
    loadWaitingExchangeRequests: () => dispatch(loadExchangeRequests()), 
    showCourseTimetable: (courseId, courseName) => dispatch(showCourseTimetable(courseId, courseName)),
    hideCourseTimetable: (courseId) => dispatch(hideCourseTimetable(courseId))
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(SidebarContainer);