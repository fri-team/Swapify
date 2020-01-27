import React, { Component } from 'react';
import { connect } from 'react-redux';
class AboutUsPage extends Component {
  
  render() {
    return (
      <div className="AboutUsContainer">
        <h1>O nás</h1>
        <p>
          Sme študenti Fakulty riadenia a informatiky. Našou snahou je vytvorenie jedinečnej aplikácie pre študentov Žilinskej univerzity (najskôr však iba našej fakulty),
          kde si budú môcť meniť svoj rozvrh a jednotlivé predmety. Ideou je to, aby študenti nemuseli hľadať niekoho, kto si taktiež bude chcieť vymeniť predmet v rozvrhu,
          ale budú si môcť predmety vymeniť jednoducho cez aplikáciu. Malo by to byť miestom, kde si môžeš predmet zmeniť a systém za Teba nájde osobu, ktorá chce opačnú výmenu.
        </p>
        <p>
          We are students from Faculty of informatics and management science. We are trying to create an unique app for students of University of Žilina (firstly just our faculty), 
          where they can change their timetables and each subjects. The whole idea is that students don´t have to seek for someone who want to change their subject in timetable too, 
          but they can easily change it via the app. It should be the place where you can make changes, and system will find you another person who wants reverse change.
        </p>
      </div>
    );
  }
}

export default connect()(AboutUsPage);
