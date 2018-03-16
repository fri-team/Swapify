import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { concat, map, times } from 'lodash';
import './Timetable.scss';

export default class Timetable extends Component {
  constructor() {
    super();
  }

  createBlock = () => {
    const cols = map(this.props.colHeadings, (col, idx) => (
      <div key={`b${idx}`} className="block" style={{ gridColumn: idx + 2 }}>{col}</div>
    ));
    const rows = map(this.props.rowHeadings, (row, idx) => (
      <div key={`d${idx}`} className="day" style={{ gridRow: idx + 2 }}>{row}</div>
    ));
    const emptyBlocks = times(cols.length * rows.length, (idx) => {
      const row = Math.floor(idx / cols.length) + 2;
      const col = (idx % cols.length) + 2;
      return (<div key={`e${idx + 1}`} className="border" style={{ gridRow: row , gridColumn: col }}></div>);
    });
    const first = (<div key="e0" className="border" style={{ gridRow: 1 , gridColumn: 1 }}></div>);
    return concat(first, cols, rows, emptyBlocks);
  }

  render() {
    const blocks = this.createBlock();
    const style = {
      gridTemplateColumns: `auto repeat(${this.props.colHeadings.length}, 1fr)`,
      gridTemplateRows: `auto repeat(${this.props.rowHeadings.length}, 1fr)`,
    };
    return (
      <div className="timetable" style={style}>
        {blocks}
        {this.props.blocks}
      </div>
    );
  }
}

Timetable.propTypes = {
  colHeadings: PropTypes.arrayOf(PropTypes.string).isRequired,
  rowHeadings: PropTypes.arrayOf(PropTypes.string).isRequired,
  blocks: PropTypes.arrayOf(PropTypes.element),
};

Timetable.defaultProps = {
  blocks: [],
};
