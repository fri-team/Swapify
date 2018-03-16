import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import Timetable from '../../components/Timetable/Timetable';

class TimetableContainer extends Component {
  render() {
    return (
      <Timetable
        colHeadings={this.props.colHeadings}
        rowHeadings={this.props.rowHeadings}
        items={this.props.blocks}
      />
    );
  }
}

TimetableContainer.propTypes = {
  colHeadings: PropTypes.arrayOf(PropTypes.string).isRequired,
  rowHeadings: PropTypes.arrayOf(PropTypes.string).isRequired,
  blocks: PropTypes.arrayOf(PropTypes.shape({})),
};

Timetable.defaultProps = {
  blocks: [],
};

const mapStateToProps = (state) => ({
  ...state.blocks,
});

export default connect(mapStateToProps)(TimetableContainer);
