import React from 'react';
import PropTypes from 'prop-types';

const Location = (props) => (
  <svg viewBox="0 0 24 24" height="24" className={props.className} fill={props.fill}>
    <path d="M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z" />
    <path d="M0 0h24v24H0z" fill="none" />
  </svg>
);

Location.propTypes = {
  className: PropTypes.string,
  fill: PropTypes.string,
};

Location.defaultProps = {
  className: '',
  fill: '#000',
};

export default Location;
