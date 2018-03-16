import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { map } from 'lodash';
import { connect } from 'react-redux';
import Timetable from '../../components/Timetable/Timetable';
import TimetableBlock from '../../components/Timetable/TimetableBlock';

class TimetableContainer extends Component {
  createBlocks = () => {
    return map(this.props.blocks, (block, idx) => (
      <TimetableBlock
        key={idx}
        name={block.name}
        room={block.room}
        teacher={block.tearcher}
        style={{
          gridColumn: `${block.startBlock + 1} / ${block.endBlock + 1}`,
          gridRow: block.day + 1,
        }}
      />
    ));
  }

  render() {
    const blocks = this.createBlocks();
    return (
      <Timetable
        colHeadings={this.props.colHeadings}
        rowHeadings={this.props.rowHeadings}
        blocks={blocks}
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
