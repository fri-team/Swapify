import React from 'react';
import { Link } from 'react-router-dom';

const HomePage = () => {
  return (
    <div>
      <h1>Home Page</h1>
      <Link to="/register">Registration Page</Link>
      <br />
      <Link to="/timetable">Timetable Page</Link>
    </div>
  );
};

export default HomePage;
