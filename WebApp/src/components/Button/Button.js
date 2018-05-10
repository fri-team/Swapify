import React from 'react';
import PropTypes from 'prop-types';
import './Button.scss';

const Button = (props) => (
  <button className="button" style={props.style} onClick={props.onClick}>
    {props.children}
  </button>
);

Button.propTypes = {
  children: PropTypes.string,
  onClick: PropTypes.func,
  style: PropTypes.shape(),
};

Button.defaultProps = {
  children: '',
  onClick: () => {},
  style: {},
};

export default Button;
