import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import './AddBlockForm.scss';

class AddBlockForm extends PureComponent {
  //handleClickOutside = () => this.props.onOutsideClick();

  render() {
    const {isVisible, startTime} = this.props;

    return (
      <div className="add-block-form">
        <p>{isVisible} = {startTime}</p>
      </div>
    );
  }
}

AddBlockForm.propTypes = {
  isVisible: PropTypes.string.isRequired,
  startTime: PropTypes.string.isRequired
};

export default AddBlockForm;