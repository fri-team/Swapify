import React, { Component } from 'react';
import PropTypes from 'prop-types';
import classNames from 'classnames';
import onClickOutside from 'react-onclickoutside';
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
    const style = {
      top: `${this.props.top}px`,
      left: `${this.props.left}px`,
    };
    const classes = classNames('block-detail', { visible: this.props.isVisible });
    return (
      <div className={classes} style={style}>
        <div className="header">
          <div className="buttons">
            <SvgButton white={true}><Delete /></SvgButton>
            <SvgButton white={true}><Cancel /></SvgButton>
          </div>
          <div className="name">Teória informácie</div>
        </div>
        <div className="footer">
          <div className="line">
            <SwapHorizontal className="icon" />
            <div className="text">
              <div className="medium">Vymieňam za Pondelok 13:00</div>
            </div>
          </div>
          <div className="line">
            <Location className="icon" />
            <div className="text">
              <div className="medium">FRI, RA301</div>
              <div className="small">kapacita 20</div>
            </div>
          </div>
          <div className="line">
            <Person className="icon" />
            <div className="text">
              <div className="medium">Ing. Tomáš Majer, PhD.</div>
              <div className="small">tomas.majer@fri.uniza.sk</div>
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
  onOutsideClick: PropTypes.func.isRequired,
};

export default onClickOutside(BlockDetail);
