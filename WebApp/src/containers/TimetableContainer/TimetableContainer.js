import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { concat, differenceWith, flatten, isEqual, map, merge, pick } from 'lodash';
import Timetable from '../../components/Timetable/Timetable';

class TimetableContainer extends Component {
  render() {
    const myBlocks = this.props.blocks;
    const subjects = map(pick(this.props.subjects, this.props.showSubjects), subject => differenceWith(subject, myBlocks, isEqual));
    const blocks = map(flatten(subjects), block => merge({}, block, { type: block.type + ' pale' }));
    const items = concat(myBlocks, blocks);
    return (
      <Timetable
        colHeadings={this.props.colHeadings}
        rowHeadings={this.props.rowHeadings}
        items={items}
      />
    );
  }
}

TimetableContainer.propTypes = {
  colHeadings: PropTypes.arrayOf(PropTypes.string).isRequired,
  rowHeadings: PropTypes.arrayOf(PropTypes.string).isRequired,
  blocks: PropTypes.arrayOf(PropTypes.shape({})).isRequired,
  subjects: PropTypes.PropTypes.shape({}).isRequired,
  showSubjects: PropTypes.arrayOf(PropTypes.string).isRequired,
};

const mapStateToProps = (state) => ({
  ...state.blocks,
});

export default connect(mapStateToProps)(TimetableContainer);
