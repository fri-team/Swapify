import React, { Component } from 'react';
import { connect } from 'react-redux';
import GlLogo from '../../images/GlobalLogic-logo-black.png';
import UnizaLogo from '../../images/Uniza-logo-black.png';

class AboutUsPage extends Component {
  constructor() {
    super();
  }

  render() {
    return (
      <div className="AboutUsContainer">
        <h1>O nás</h1>
        <p>
          Sme študenti Fakulty riadenia a informatiky. Našou snahou je vytvorenie <strong>jedinečnej aplikácie</strong> pre študentov Žilinskej univerzity (najskôr však iba našej fakulty),
          kde si budú môcť meniť svoj rozvrh a jednotlivé predmety. Ideou je to, aby študenti nemuseli hľadať niekoho, kto si taktiež bude chcieť <strong>vymeniť predmet v rozvrhu</strong>,
          ale budú si môcť predmety vymeniť jednoducho cez aplikáciu. Malo by to byť miestom, kde si môžeš predmet zmeniť a systém za Teba nájde osobu, ktorá chce opačnú výmenu.
        </p>
        <p>
          We are students from Faculty of informatics and management science. We are trying to create an <strong>unique app</strong> for students of University of Žilina (firstly just our faculty),
          where they can change their timetables and each subjects. The whole idea is that students don´t have to seek for someone who want to <strong>change their subject in timetable</strong> too,
          but they can easily change it via the app. It should be the place where you can make changes, and system will find you another person who wants reverse change.
        </p>
        <div className="AboutUsLogos">
          <img src={GlLogo} alt="Global Logic logo" width="807" height="151" />
          <img src={UnizaLogo} alt="Uniza logo" width="4082" height="4096" />
        </div>
      </div>
    );
  }
}

export default connect()(AboutUsPage);
