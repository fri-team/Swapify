import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import * as actions from '../../actions/blockDetailActions';
import BlockDetail from '../../components/BlockDetail/BlockDetail';
import * as timetableActions from '../../actions/timetableActions';

class BlockDetailContainer extends Component {
  state = {
    user: this.props.user,
    timetableType: this.props.timetableType
  };

  onOutsideClick = () => {
    this.props.actions.hideDetail();
  }

  exchangeRequest = (course) =>
  {
    this.props.timetableActions.showExchangeModeTimetable(course);
    this.props.timetableActions.chooseExchangeFromBlock(course);
    this.props.actions.hideDetail();
  }

  onClickDelete = (block) => {
    this.props.timetableActions.removeBlock(block, this.state.user.email);
    this.props.actions.hideDetail();
  }

  onClickEdit = () => {
    this.props.actions.hideDetail();
  }

  onClickAdd = (block) => {
    this.props.timetableActions.addBlockAndHideOthersWithSameCourseId(block, this.state.user.email);
    this.props.actions.hideDetail();
  }

  render() {
    const { isVisible, top, left, course } = this.props;
    return (
      <BlockDetail
        isVisible={isVisible}
        top={top}
        left={left}
        course={course}
        user={this.state.user}
        onOutsideClick={this.onOutsideClick}
        onExchangeRequest={this.exchangeRequest}
        onClickDelete={this.onClickDelete}
        onClickEdit={this.onClickEdit}
        onClickAdd={this.onClickAdd}
        timetableType={this.state.timetableType}
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
