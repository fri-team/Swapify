import React from 'react';
import PropTypes from 'prop-types';
import { map, merge } from 'lodash';
import TimetableBlocks from './TimetableBlocks';
import './Timetable.scss';

const Timetable = (props) => {
  const colHeadings = map(props.colHeadings, (col, idx) => (
    <div key={idx} className="block">{col}</div>
  ));
  const rowHeadings = map(props.rowHeadings, (row, idx) => (
    <div key={idx} className="day">{row}</div>
  ));
  const blocks = [];
  for (let i = 1; i <= rowHeadings.length; i++) {
    for (let j = 1; j <= colHeadings.length; j++) {
      blocks.push(
        <div key={`${i}x${j}`} className="cell" style={{
          gridRow: i,
          gridColumn: j,
        }} />
      );
    }
  }
  const colStyle = { gridTemplateColumns: `repeat(${colHeadings.length}, 1fr)` };
  const rowStyle = { gridTemplateRows: `repeat(${rowHeadings.length}, 1fr)` };
  const style = merge({}, colStyle, rowStyle);
  return (
    <div className="timetable">
      <div className="empty-cell"></div>
      <div className="col-head" style={colStyle}>{colHeadings}</div>
      <div className="row-head" style={rowStyle}>{rowHeadings}</div>
      <div className="cells" style={style}>{blocks}</div>
      <TimetableBlocks
        columns={colHeadings.length}
        rows={rowHeadings.length}
        items={props.items}
      />
    </div>
  );
};

Timetable.propTypes = {
  colHeadings: PropTypes.arrayOf(PropTypes.string).isRequired,
  rowHeadings: PropTypes.arrayOf(PropTypes.string).isRequired,
  items: PropTypes.arrayOf(PropTypes.shape({})),
};

Timetable.defaultProps = {
  items: [],
};

export default Timetable;
