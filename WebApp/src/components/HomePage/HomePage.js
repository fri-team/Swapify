import React from 'react';
import Button from '@material-ui/core/Button';
import './HomePage.scss';

const HomePage = ({ history }) => (
  <div className="container home">
    <div className="home-wrapper">
      <h1>Swapify</h1>
      <h3>Spravuj svoj rozvrh na jednom mieste!</h3>
      <Button
        color="primary"
        variant="contained"
        onClick={() => history.push('/register')}
      >
        Registrovať sa
      </Button>
      <Button
        color="primary"
        variant="contained"
        onClick={() => history.push('/login')}
      >
        Prihlásiť sa
      </Button>
    </div>
  </div>
);

export default HomePage;
