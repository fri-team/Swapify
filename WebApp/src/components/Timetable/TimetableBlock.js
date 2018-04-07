import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';
import toMaterialStyle from 'material-color-hash';
import Lecture from '../svg/Lecture';
import Laboratory from '../svg/Laboratory';
import './TimetableBlock.scss';

const TimetableBlock = (props) => {
  const { backgroundColor, color } = toMaterialStyle(props.name);
  const myStyle = {
    ...props.style,
    backgroundColor,
    color,
  };
  const icon = props.type === 'lecture' ? <Lecture fill={myStyle.color} /> : <Laboratory fill={myStyle.color} />;
  return (
    <div className={classNames('block', props.cssClasses)} style={myStyle}>
      <div>
        <div className="name">{props.name}</div>
        {icon}
      </div>
      <div className="room">{props.room}</div>
      <div className="teacher">{props.teacher}</div>
    </div>
  );
};

TimetableBlock.propTypes = {
  name: PropTypes.string.isRequired,
  room: PropTypes.string.isRequired,
  teacher: PropTypes.string.isRequired,
  type: PropTypes.string.isRequired,
  cssClasses: PropTypes.arrayOf(PropTypes.string),
  style: PropTypes.shape({}),
};

TimetableBlock.defaultProps = {
  cssClasses: [],
  style: {},
};

export default TimetableBlock;
