import React, { Component } from 'react';
import PropTypes from 'prop-types';
import onClickOutside from 'react-onclickoutside';
import { deburr, lowerCase, replace } from 'lodash';
import toMaterialStyle from 'material-color-hash';
import SvgButton from '../SvgButton/SvgButton';
import Cancel from '../svg/Cancel';
import Delete from '../svg/Delete';
import SwapHorizontal from '../svg/SwapHorizontal';
import Location from '../svg/Location';
import Person from '../svg/Person';
import './BlockDetail.scss';

class BlockDetail extends Component {
  handleClickOutside = () => {
    this.props.onOutsideClick();
  }

  render() {
    if (!this.props.isVisible) {
      return null;
    }
    const { top, left, course } = this.props;
    const email = replace(lowerCase(deburr(course.teacher)), ' ', '.') + '@fri.uniza.sk';
    const { backgroundColor, color } = toMaterialStyle(course.courseShortcut || '');
    const hoverColor = replace(color, /[^,]+\)/, '.2)');
    const style = { top: `${top}px`, left: `${left}px` };
    return (
      <div className="block-detail" style={style}>
        <div className="header" style={{ backgroundColor }}>
          <div className="buttons">
            {course.type !== 'lecture' && <SvgButton hoverColor={hoverColor}><Delete fill={color} /></SvgButton>}
            <SvgButton hoverColor={hoverColor} onClick={this.handleClickOutside}>
              <Cancel fill={color} />
            </SvgButton>
          </div>
          <div className="name" style={{ color }}>{course.courseName}</div>
        </div>
        <div className="footer">
          {course.type !== 'lecture' && <div className="line">
            <SwapHorizontal className="icon" />
            <div className="text">
              <div className="medium">Vymieňam za Pondelok 13:00</div>
            </div>
          </div>}
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
};

export default onClickOutside(BlockDetail);
