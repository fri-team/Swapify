import React from 'react';
import classNames from 'classnames';
import './ElevatedBox.scss';

const ElevatedBox = ({ children, className }) => (
<div className="App">
          <div className="App__Aside"></div>
          <div className="App__Form">
           {children}
          </div>

        </div>
);

export default ElevatedBox;
