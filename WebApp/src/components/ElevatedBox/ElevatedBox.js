    
import React from 'react';
import classNames from 'classnames';
import './ElevatedBox.scss';

const ElevatedBox = ({ children, className }) => (
  <div className={classNames('centered-box', className)}>
    <div className="centered-box__content">{children}</div>
  </div>
);

export default ElevatedBox;