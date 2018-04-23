import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../actions/blockDetailActions';
import TimetableBlock from '../../components/Timetable/TimetableBlock';

class TimetableBlockContainer extends Component {
  onBlockClick = (evt) => {
    const { offsetTop, offsetLeft, offsetWidth } = evt.currentTarget;
    this.props.actions.showDetail(offsetTop, offsetLeft + offsetWidth + 8);
  }

  render() {
    return (
      <TimetableBlock
        {...this.props.passedProps}
        onClick={this.onBlockClick}
      />
    );
  }
}

TimetableBlockContainer.propTypes = {
  passedProps: PropTypes.shape({}).isRequired,
  actions: PropTypes.shape({
    showDetail: PropTypes.func,
  }).isRequired,
};

TimetableBlockContainer.defaultProps = {};

const mapStateToProps = (state, ownProps) => ({
  ...state.blockDetail,
  passedProps: ownProps,
});

const mapDispatchToProps = (dispatch) => ({
  actions: bindActionCreators(actions, dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(TimetableBlockContainer);
