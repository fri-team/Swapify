import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';
import './TimetableBlock.scss';

const TimetableBlock = (props) => (
  <div className={classNames('block', props.cssClasses)} style={props.style}>
    <div className="name">{props.name}</div>
    <div className="room">{props.room}</div>
    <div className="teacher">{props.teacher}</div>
  </div>
);

TimetableBlock.propTypes = {
  name: PropTypes.string.isRequired,
  room: PropTypes.string.isRequired,
  teacher: PropTypes.string.isRequired,
  cssClasses: PropTypes.arrayOf(PropTypes.string),
  style: PropTypes.shape({}),
};

TimetableBlock.defaultProps = {
  cssClasses: [],
  style: {},
};

export default TimetableBlock;
