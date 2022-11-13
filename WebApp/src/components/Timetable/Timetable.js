import React from 'react';
import { map, merge } from 'lodash';
import { connect } from 'react-redux';
import TimetableBlocks from './TimetableBlocks';
import AddBlockForm from '../AddBlockForm/AddBlockForm'
import './Timetable.scss';

class Timetable extends React.Component {
  state = {
    course: {
      courseName: '',
      courseCode: '',
      courseShortcut: '',
      teacher: '',
      room: '',
      day: null,
      startBlock: null,
      length: 2,
      type: '',
    },
    day: null,
    start: null,
    dialogOpen: false
  }

  openAddBlock = (i, j) => {
    let course = { ...this.state.course }
    course.day = i;
    course.startBlock = j;
    this.setState({ course });
    this.setState({ day: i, start: j, dialogOpen: true })
  }

  handleSumbitClick = () => {
    this.setState({ dialogOpen: false })
  };

  handleClickOutside = () => {
    this.setState({ dialogOpen: false })
  };

  render() {
    const Block = ({ i, j, openAddBlock }) => (
      <div
        className={this.props.darkMode ? "border-cell-dark" : "border-cell" }
        style={{ gridRow: i, gridColumn: j }}
        onClick={() => openAddBlock(i, j)}
      />
    );
    const { colHeadings, rowHeadings, items, user } = this.props
    const { dialogOpen, course } = this.state
    const hours = map(colHeadings, (col, idx) => (
      <div key={idx} className={this.props.darkMode ? "border-cell-dark centered" : "border-cell centered" }>
        {col}
      </div>
    ));
    const days = map(rowHeadings, (row, idx) => (
      <div key={idx} className={this.props.darkMode ? "border-cell-dark" : "border-cell" }>
        {row}
      </div>
    ));
    const borderCells = [];
    for (let i = 1; i <= days.length; i++) {
      for (let j = 1; j <= hours.length; j++) {
        borderCells.push(
          <Block key={`${i}x${j}`} i={i} j={j} openAddBlock={this.openAddBlock} className={this.props.darkMode ? "border-cell-dark" : "border-cell" }/>
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
    const style = merge({}, hoursStyle, daysStyle);

    return (
      <div className="timetable">
        <div className={this.props.darkMode ? "border-cell-dark" : "border-cell" }/>
        <div style={hoursStyle}>{hours}</div>
        <div style={daysStyle}>{days}</div>
        <TimetableBlocks
          columns={hours.length}
          rows={days.length}
          items={items}
        />
        <div className={ this.props.darkMode ? "border-cells-dark" : "border-cells" } style={style}>
          {borderCells}
        </div>
        {dialogOpen && (
          <AddBlockForm
            user={user}
            course={course}
            onSubmitClick={this.handleSumbitClick}
            onCloseEditBlock={this.handleSumbitClick}
            onClose={this.handleClickOutside}
            editing={false}
          />
        )}
      </div>
    );
  }
}

export default connect(state => ({
  user: state.user
}))(Timetable);
