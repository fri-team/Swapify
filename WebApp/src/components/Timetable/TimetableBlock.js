import React from 'react';
import PropTypes from 'prop-types';
import './TimetableBlock.scss';

const TimetableBlock = (props) => (
  <div className={`block ${props.type}`} style={props.style}>
    <div className="name">{props.name}</div>
    <div className="room">{props.room}</div>
    <div className="teacher">{props.teacher}</div>
  </div>
);

TimetableBlock.propTypes = {
  name: PropTypes.string.isRequired,
  room: PropTypes.string.isRequired,
  teacher: PropTypes.string.isRequired,
  type: PropTypes.string,
  style: PropTypes.shape({}),
};

TimetableBlock.defaultProps = {
  type: '',
  style: {},
};

export default TimetableBlock;
