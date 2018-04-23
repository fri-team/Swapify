import React from 'react';
import PropTypes from 'prop-types';

const Person = (props) => (
  <svg viewBox="0 0 24 24" height="24" className={props.className} fill={props.fill}>
    <path d="M12 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm0 2c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z" />
    <path d="M0 0h24v24H0z" fill="none" />
  </svg>
);

Person.propTypes = {
  className: PropTypes.string,
  fill: PropTypes.string,
};

Person.defaultProps = {
  className: '',
  fill: '#000',
};

export default Person;
