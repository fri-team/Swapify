import React, { Component } from 'react';
import PropTypes from 'prop-types';
import HamburgerButton from '../HamburgerButton/HamburgerButton';
import UserButton from '../UserButton/UserButton';
import './Toolbar.scss';

export default class Toolbar extends Component {
  render() {
    return (
      <div className="toolbar">
        <HamburgerButton onClick={this.props.toggleSidebar} />
        <UserButton />
      </div>
    );
  }
}

Toolbar.propTypes = {
  toggleSidebar: PropTypes.func.isRequired,
};

Toolbar.defaultProps = {
};
