import React from 'react';
import PropTypes from 'prop-types';

const Delete = (props) => (
  <svg viewBox="0 0 24 24" fill={props.fill} height="24">
    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z" />
    <path d="M0 0h24v24H0z" fill="none" />
  </svg>
);

Delete.propTypes = {
  fill: PropTypes.string,
};

Delete.defaultProps = {
  fill: '#000',
};

export default Delete;