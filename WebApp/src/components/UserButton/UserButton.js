import React, { Component } from 'react';
import './UserButton.scss';

export default class UserButton extends Component {
  render() {
    return (
      <div className="user">
        <div className="info">
          <div className="name">Mikulas Zaymus</div>
          <div className="email">mikulas.zaymus@st.fri.uniza.sk</div>
        </div>
        <div className="icon">M</div>
      </div>
    );
  }
}
