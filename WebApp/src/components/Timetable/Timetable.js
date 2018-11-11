import React from 'react';
import PropTypes from 'prop-types';
import _ from 'lodash';
import TimetableBlocks from './TimetableBlocks';
import './Timetable.scss';

const Timetable = props => {
  const hours = _.map(props.colHeadings, (col, idx) => (
    <div key={idx} className="border-cell">
      {col}
    </div>
  ));
  const days = _.map(props.rowHeadings, (row, idx) => (
    <div key={idx} className="border-cell">
      {row}
    </div>
  ));
  const borderCells = [];
  for (let i = 1; i <= days.length; i++) {
    for (let j = 1; j <= hours.length; j++) {
      borderCells.push(
        <div
          key={`${i}x${j}`}
          className="border-cell"
          style={{ gridRow: i, gridColumn: j }}
        />
      );
    }
  }
  const hoursStyle = {
    display: 'grid',
    gridTemplateColumns: `repeat(${hours.length}, 1fr)`
  };
  const daysStyle = {
    display: 'grid',
    gridTemplateRows: `repeat(${days.length}, 1fr)`
  };
  const style = _.merge({}, hoursStyle, daysStyle);
  return (
    <div className="timetable">
      <div className="border-cell" />
      <div style={hoursStyle}>{hours}</div>
      <div style={daysStyle}>{days}</div>
      <div className="border-cells" style={style}>
        {borderCells}
      </div>
      <TimetableBlocks
        columns={hours.length}
        rows={days.length}
        items={props.items}
      />
    </div>
  );
};

Timetable.propTypes = {
  colHeadings: PropTypes.arrayOf(PropTypes.string).isRequired,
  rowHeadings: PropTypes.arrayOf(PropTypes.string).isRequired,
  items: PropTypes.arrayOf(PropTypes.shape({}))
};

Timetable.defaultProps = {
  items: []
};

export default Timetable;
