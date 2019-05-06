import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../actions/blockDetailActions';
import TimetableBlock from '../../components/Timetable/TimetableBlock';
import * as timetableActions from '../../actions/timetableActions';

class TimetableBlockContainer extends Component {
  onBlockClick = (evt, course) => {
    if (this.props.timetable.isExchangeMode && !course.isMine) {
        this.props.timetableActions.exchangeConfirm(course);
    } else {
        const { offsetTop, offsetLeft, offsetWidth } = evt.currentTarget;
        this.props.actions.showDetail(offsetTop, offsetLeft + offsetWidth + 8, course);
    }
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
  timetable: state.timetable
});

const mapDispatchToProps = (dispatch) => ({
  actions: bindActionCreators(actions, dispatch),
  timetableActions: bindActionCreators(timetableActions, dispatch)
});

export default connect(mapStateToProps, mapDispatchToProps)(TimetableBlockContainer);
