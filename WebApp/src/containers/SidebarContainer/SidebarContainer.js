import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { keys, map } from 'lodash';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../actions/timetableActions';
import Sidebar from '../../components/Sidebar/Sidebar';
import ImgCheckbox from '../../components/ImgCheckbox/ImgCheckbox';

class SidebarContainer extends Component {
  render() {
    const subjects = keys(this.props.subjects);
    const checkboxes = map(subjects, (subject, idx) => (
      <ImgCheckbox
        key={idx}
        checkedImg="eye.svg"
        uncheckedImg="eye-slash.svg"
        onChange={(checked) => {
          if (checked) {
            this.props.actions.showSubject(subject);
          } else {
            this.props.actions.hideSubject(subject);
          }
        }}
      >{subject}</ImgCheckbox>
    ));
    return (
      <Sidebar>{checkboxes}</Sidebar>
    );
  }
}

SidebarContainer.propTypes = {
  subjects: PropTypes.shape({}).isRequired,
  actions: PropTypes.shape({
    showSubject: PropTypes.func,
    hideSubject: PropTypes.func,
  }).isRequired,
};

const mapStateToProps = (state) => ({
  ...state.blocks,
});

const mapDispatchToProps = (dispatch) => ({
  actions: bindActionCreators(actions, dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(SidebarContainer);
