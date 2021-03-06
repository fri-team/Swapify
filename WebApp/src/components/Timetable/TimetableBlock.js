import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';
import toMaterialStyle from 'material-color-hash';
import Lecture from '../svg/Lecture';
import Laboratory from '../svg/Laboratory';
import './TimetableBlock.scss';

const TimetableBlock = (props) => {
  const { backgroundColor, color } = toMaterialStyle(props.courseCode);
  const myStyle = {
    ...props.style,
    backgroundColor,
    color,
    zIndex: '2',
  };
  const icon = props.type === 'lecture' ? <Lecture fill={myStyle.color} /> : <Laboratory fill={myStyle.color} />;
  return (
    <div
      className={classNames('block', props.cssClasses)}
      style={myStyle}
      onClick={(event) => {
        props.onClick(event, props);
      }}
    >
      <div>
        <div className="name">{props.courseCode}</div>
        {icon}
      </div>
      <div className="room">{props.room}</div>
      <div className="teacher">{props.teacher}</div>
      {!props.isGrey && props.isMine  && <div className="opacity" />}
    </div>
  );
};

TimetableBlock.propTypes = {
  courseCode: PropTypes.string.isRequired,
  room: PropTypes.string.isRequired,
  teacher: PropTypes.string.isRequired,
  type: PropTypes.string.isRequired,
  isMine: PropTypes.bool,
  isGrey: PropTypes.bool,
  cssClasses: PropTypes.arrayOf(PropTypes.string),
  style: PropTypes.shape({}),
  onClick: PropTypes.func.isRequired,
};

TimetableBlock.defaultProps = {
  isMine: false,
  isGrey: false,
  cssClasses: [],
  style: {},
};

export default TimetableBlock;
