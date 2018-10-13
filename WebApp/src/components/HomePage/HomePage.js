import React from 'react';
import Button from '@material-ui/core/Button';
import { ElevatedBox, MacBackground } from '../';
import './HomePage.scss';

const HomePage = ({ history }) => (
  <MacBackground>
    <ElevatedBox className="home-content">
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
    </ElevatedBox>
  </MacBackground>
);

export default HomePage;
