import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../actions/timetableActions';
import Timetable from '../../components/Timetable/Timetable';

class TimetableContainer extends Component {
  constructor(props) {
    super(props);

    this.props.actions.loadMyTimetable(this.props.user, this.props.history);
  }

  render() {
    return (
      <Timetable
        darkMode={this.props.darkMode}
        colHeadings={this.props.colHeadings}
        rowHeadings={this.props.rowHeadings}
        items={this.props.displayedTimetable}
      />
    );
  }
}

TimetableContainer.propTypes = {
  colHeadings: PropTypes.arrayOf(PropTypes.string).isRequired,
  rowHeadings: PropTypes.arrayOf(PropTypes.string).isRequired,
  displayedTimetable: PropTypes.arrayOf(PropTypes.shape({})).isRequired,
  actions: PropTypes.shape({
    loadMyTimetable: PropTypes.func,
  }).isRequired,
};

const mapStateToProps = (state) => ({
  ...state.timetable,
  user: state.user,
});

const mapDispatchToProps = (dispatch) => ({
  actions: bindActionCreators(actions, dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(TimetableContainer);
