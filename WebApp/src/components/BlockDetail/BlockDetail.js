import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import onClickOutside from 'react-onclickoutside';
import _ from 'lodash';
import toMaterialStyle from 'material-color-hash';
import IconButton from '@material-ui/core/IconButton';
import DeleteIcon from '@material-ui/icons/Delete';
import ClearIcon from '@material-ui/icons/Clear';
import SwapHorizontal from '../svg/SwapHorizontal';
import Location from '../svg/Location';
import Person from '../svg/Person';
import './BlockDetail.scss';

class BlockDetail extends PureComponent {
  handleClickOutside = () => this.props.onOutsideClick();
  handleClickExchange = () => this.props.onExchangeRequest(this.props.course);
  handleClickDelete = () => this.props.onClickDelete(this.props.course);

  render() {
    if (!this.props.isVisible) {
      return null;
    }
    const { top, left, course } = this.props;
    const email =
      _.replace(_.lowerCase(_.deburr(course.teacher)), ' ', '.') +
      '@fri.uniza.sk';
    const { backgroundColor, color } = toMaterialStyle(
      course.courseShortcut || ''
    );
    const style = { top: `${top}px`, left: `${left}px` };
    return (
      <div className="block-detail" style={style}>
        <div className="header" style={{ backgroundColor }}>
          <div className="buttons">
            {course.type !== 'lecture' && (
              <span>
                <IconButton onClick={this.handleClickExchange }>
                  <SwapHorizontal className="icon" />
                </IconButton>
                <IconButton onClick={this.handleClickDelete}>
                  <DeleteIcon nativeColor={color} />
                </IconButton>
              </span>
            )}
            <IconButton onClick={this.handleClickOutside}>
              <ClearIcon nativeColor={color} />
            </IconButton>
          </div>
          <div className="name" style={{ color }}>
            {course.courseName}
          </div>
        </div>
        <div className="footer">
          {course.type !== 'lecture' && (
            <div className="line">
              <SwapHorizontal className="icon" />
              <div className="text">
                <div className="medium">Vymieňam za Pondelok 13:00</div>
              </div>
            </div>
          )}
          <div className="line">
            <Location className="icon" />
            <div className="text">
              <div className="medium">{`FRI, ${course.room}`}</div>
              <div className="small">kapacita 20</div>
            </div>
          </div>
          <div className="line">
            <Person className="icon" />
            <div className="text">
              <div className="medium">{course.teacher}</div>
              <div className="small">{email}</div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

BlockDetail.propTypes = {
  isVisible: PropTypes.bool.isRequired,
  top: PropTypes.number.isRequired,
  left: PropTypes.number.isRequired,
  course: PropTypes.shape({}).isRequired,
  onOutsideClick: PropTypes.func.isRequired,
  onExchangeRequest: PropTypes.func.isRequired
};

export default onClickOutside(BlockDetail);
