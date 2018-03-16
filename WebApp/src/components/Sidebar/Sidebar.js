import React from 'react';
import PropTypes from 'prop-types';
import './Sidebar.scss';

const Sidebar = (props) => (
  <div className="sidebar">
    {props.children}
  </div>
);

Sidebar.propTypes = {
  children: PropTypes.any,
};

Sidebar.defaultProps = {
  children: null,
};

export default Sidebar;
