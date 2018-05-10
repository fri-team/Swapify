import React from 'react';
import PropTypes from 'prop-types';
import Button from '../Button/Button';
import './HomePage.scss';

const HomePage = (props) => {
  return (
    <div className="container home">
      <div className="home-wrapper">
        <h1>Swapify</h1>
        <h3>Spravuj svoj rozvrh na jednom mieste!</h3>
        <Button onClick={() => {
          props.history.push('/register');
        }}>Registrovať sa</Button>
        <Button onClick={() => {
          props.history.push('/login');
        }}>Prihlásiť sa</Button>
      </div>
    </div>
  );
};


HomePage.propTypes = {
  history: PropTypes.shape({
    push: PropTypes.func,
  }).isRequired,
};

export default HomePage;
