import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { includes, map } from 'lodash';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../actions/timetableActions';
import Sidebar from '../../components/Sidebar/Sidebar';
import ImgCheckbox from '../../components/ImgCheckbox/ImgCheckbox';

class SidebarContainer extends Component {
  render() {
    const checkboxes = map(this.props.myCourseNames, (course, idx) => (
      <ImgCheckbox
        key={idx}
        checkedImg="eye.svg"
        uncheckedImg="eye-slash.svg"
        checked={includes(this.props.displayedCourses, course)}
        onChange={(checked) => {
          if (checked) {
            this.props.actions.showCourseTimetable(course);
          } else {
            this.props.actions.hideCourseTimetable(course);
          }
        }}
      >{course}</ImgCheckbox>
    ));
    return (
      <Sidebar open={this.props.open}>{checkboxes}</Sidebar>
    );
  }
}

SidebarContainer.propTypes = {
  open: PropTypes.bool.isRequired,
  myCourseNames: PropTypes.arrayOf(PropTypes.string).isRequired,
  displayedCourses: PropTypes.arrayOf(PropTypes.string).isRequired,
  actions: PropTypes.shape({
    showCourseTimetable: PropTypes.func,
    hideCourseTimetable: PropTypes.func,
  }).isRequired,
};

const mapStateToProps = (state, ownProps) => ({
  ...ownProps,
  ...state.timetable,
});

const mapDispatchToProps = (dispatch) => ({
  actions: bindActionCreators(actions, dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(SidebarContainer);
