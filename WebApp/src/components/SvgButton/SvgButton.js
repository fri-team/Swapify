import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';
import './SvgButton.scss';

const SvgButton = (props) => {
  const cls = classNames('svg-button', { white: props.white });
  return (
    <div className={cls} onClick={props.onClick}>
      {props.children}
    </div>
  );
};

SvgButton.propTypes = {
  children: PropTypes.element.isRequired,
  white: PropTypes.bool,
  onClick: PropTypes.func,
};

SvgButton.defaultProps = {
  white: false,
  onClick: () => { },
};

export default SvgButton;
