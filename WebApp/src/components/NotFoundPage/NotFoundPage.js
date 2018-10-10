import React from 'react';
import { Link } from 'react-router-dom';
import './NotFoundPage.scss';

const NotFoundPage = () => {
  return (
    <div className="container not-found-background">
      <div className="not-found-wrapper">
        <div className="not-found">404</div>
        <Link to="/">Go Home</Link>
      </div>
    </div>
  );
};

export default NotFoundPage;
