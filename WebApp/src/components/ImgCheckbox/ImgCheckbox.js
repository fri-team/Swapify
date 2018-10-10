import React, { Component } from 'react';
import PropTypes from 'prop-types';
import './ImgCheckbox.scss';

export default class ImgCheckbox extends Component {
  state = {
    checked: this.props.checked,
  }

  onChange = () => {
    const checked = !this.state.checked;
    this.setState({ checked });
    this.props.onChange(checked);
  }

  render() {
    const { checked } = this.state;
    const { checkedImg, uncheckedImg, style } = this.props;
    const labelStyle = {
      ...style,
      backgroundImage: `url(../../images/${checked ? checkedImg : uncheckedImg})`,
    };
    return (
      <label className="img-checkbox" style={labelStyle}>
        <input
          type="checkbox"
          checked={checked}
          onChange={this.onChange}
        />
        {this.props.children}
      </label>
    );
  }
}

ImgCheckbox.propTypes = {
  children: PropTypes.string,
  checked: PropTypes.bool,
  checkedImg: PropTypes.string,
  uncheckedImg: PropTypes.string,
  style: PropTypes.shape({}),
  onChange: PropTypes.func,
};

ImgCheckbox.defaultProps = {
  children: '',
  checked: false,
  checkedImg: 'eye.svg',
  uncheckedImg: 'eye-slash.svg',
  style: {},
  onChange: () => {},
};
