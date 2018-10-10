import React, { Component } from 'react';
import PropTypes from 'prop-types';
import './SvgButton.scss';

export default class SvgButton extends Component {
  state = {
    hovered: false,
  }

  render() {
    let style = {};
    if (this.state.hovered) {
      style = { backgroundColor: this.props.hoverColor };
    }
    return (
      <div
        className="svg-button"
        style={style}
        onClick={this.props.onClick}
        onMouseEnter={() => {
          this.setState({ hovered: true });
        }}
        onMouseLeave={() => {
          this.setState({ hovered: false });
        }}
      >
        {this.props.children}
      </div>
    );
  }
}

SvgButton.propTypes = {
  children: PropTypes.element.isRequired,
  hoverColor: PropTypes.string.isRequired,
  onClick: PropTypes.func,
};

SvgButton.defaultProps = {
  onClick: () => { },
};
