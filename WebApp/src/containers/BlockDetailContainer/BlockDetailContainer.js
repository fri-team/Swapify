import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../actions/blockDetailActions';
import BlockDetail from '../../components/BlockDetail/BlockDetail';
import * as timetableActions from '../../actions/timetableActions';

class BlockDetailContainer extends Component {
  onOutsideClick = () => {
    this.props.actions.hideDetail();
  }

  exchangeRequest = (course) =>
  {
    this.props.timetableActions.showExchangeModeTimetable(course);
  }

  render() {
    const { isVisible, top, left, course } = this.props;
    return (
      <BlockDetail
        isVisible={isVisible}
        top={top}
        left={left}
        course={course}
        onOutsideClick={this.onOutsideClick}
        onExchangeRequest={this.exchangeRequest}
      />
    );
  }
}

BlockDetailContainer.propTypes = {
  isVisible: PropTypes.bool.isRequired,
  top: PropTypes.number.isRequired,
  left: PropTypes.number.isRequired,
  course: PropTypes.shape({}).isRequired,
  actions: PropTypes.shape({
    hideDetail: PropTypes.func,
    exchangeRequest: PropTypes.func,
  }).isRequired,
};

BlockDetailContainer.defaultProps = {};

const mapStateToProps = (state) => ({
  ...state.blockDetail,
  ...state.timetable,
});

const mapDispatchToProps = (dispatch) => ({
  actions: bindActionCreators(actions, dispatch),
  timetableActions: bindActionCreators(timetableActions, dispatch),
});

export default connect(mapStateToProps, mapDispatchToProps)(BlockDetailContainer);
