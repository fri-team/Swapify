import React, { Component } from 'react';
import PropTypes from 'prop-types';
import './Checkbox.scss';

export default class Checkbox extends Component {
  constructor(props) {
    super(props);
    this.state = {
      checked: props.checked,
    };
  }

  onChange = () => {
    this.setState({
      checked: !this.state.checked,
    });
  }

  render() {
    const svgStyle = {
      fill: this.state.checked ? this.props.checkedColor : this.props.uncheckedColor,
    };
    return (
      <div className="checkbox-eye" style={this.props.style}>
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512" height="1em" style={svgStyle}>
          <path d="M569.354 231.631C512.969 135.949 407.81 72 288 72 168.14 72 63.004 135.994 6.646 231.631a47.999 47.999 0 0 0 0 48.739C63.031 376.051 168.19 440 288 440c119.86 0 224.996-63.994 281.354-159.631a47.997 47.997 0 0 0 0-48.738zM288 392c-75.162 0-136-60.827-136-136 0-75.162 60.826-136 136-136 75.162 0 136 60.826 136 136 0 75.162-60.826 136-136 136zm104-136c0 57.438-46.562 104-104 104s-104-46.562-104-104c0-17.708 4.431-34.379 12.236-48.973l-.001.032c0 23.651 19.173 42.823 42.824 42.823s42.824-19.173 42.824-42.823c0-23.651-19.173-42.824-42.824-42.824l-.032.001C253.621 156.431 270.292 152 288 152c57.438 0 104 46.562 104 104z"/>
        </svg>
        <label>
          <input
            type="checkbox"
            checked={this.state.checked}
            onChange={this.onChange}
          />
          {this.props.children}
        </label>
      </div>
    );
  }
}

Checkbox.propTypes = {
  children: PropTypes.string,
  checked: PropTypes.bool,
  checkedColor: PropTypes.string,
  uncheckedColor: PropTypes.string,
  style: PropTypes.shape({}),
};

Checkbox.defaultProps = {
  children: '',
  checked: false,
  checkedColor: '#000000',
  uncheckedColor: '#e0e0e0',
  style: {},
};
