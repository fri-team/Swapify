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
        if (offsetWidth > 900) {
          this.props.actions.showDetail(offsetTop, offsetLeft + (window.innerWidth * 0.2) - 8 , course);
        } else {
          if ((offsetLeft + offsetWidth + 8 + (window.innerWidth * 0.2) > window.innerWidth) && (offsetTop > window.innerHeight * 0.75)){
            this.props.actions.showDetail(offsetTop, offsetLeft - (window.innerWidth * 0.2) - 8 , course);
          } else if (offsetTop > window.innerHeight * 0.75) {
            this.props.actions.showDetail(offsetTop, offsetLeft + offsetWidth + 8, course);
          } else if (offsetLeft + offsetWidth + 8 + (window.innerWidth * 0.2) > window.innerWidth) {
            this.props.actions.showDetail(offsetTop, offsetLeft - (window.innerWidth * 0.2) - 8 , course);
          } else {
            this.props.actions.showDetail(offsetTop, offsetLeft + offsetWidth + 8, course);
          }
        }      
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
