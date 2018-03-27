import React from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';
import './Sidebar.scss';

const Sidebar = (props) => {
  const sidebar = classNames('sidebar', { open: props.open });
  return (
    <div className={sidebar}>
      {props.children}
    </div>
  );
};

Sidebar.propTypes = {
  open: PropTypes.bool.isRequired,
  children: PropTypes.any,
};

Sidebar.defaultProps = {
  children: null,
};

export default Sidebar;
