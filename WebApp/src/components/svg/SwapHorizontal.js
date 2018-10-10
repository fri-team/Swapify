import React from 'react';
import PropTypes from 'prop-types';

const SwapHorizontal = (props) => (
  <svg viewBox="0 0 24 24" height="24" className={props.className} fill={props.fill}>
    <path d="M6.99 11L3 15l3.99 4v-3H14v-2H6.99v-3zM21 9l-3.99-4v3H10v2h7.01v3L21 9z" />
    <path d="M0 0h24v24H0z" fill="none" />
  </svg>
);

SwapHorizontal.propTypes = {
  className: PropTypes.string,
  fill: PropTypes.string,
};

SwapHorizontal.defaultProps = {
  className: '',
  fill: '#000',
};

export default SwapHorizontal;
