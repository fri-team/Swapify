import React from 'react';
import PropTypes from 'prop-types';

const TimetableBlock = (props) => (
  <div className="lab" style={props.style}>
    <div className="name">{props.name}</div>
    <div className="room">{props.room}</div>
    <div className="teacher">{props.teacher}</div>
  </div>
);

TimetableBlock.propTypes = {
  name: PropTypes.string.isRequired,
  room: PropTypes.string.isRequired,
  teacher: PropTypes.string.isRequired,
  style: PropTypes.shape({}),
};

TimetableBlock.defaultProps = {
  style: {},
};

export default TimetableBlock;
