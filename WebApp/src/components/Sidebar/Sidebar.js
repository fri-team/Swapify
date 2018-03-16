import React from 'react';
import PropTypes from 'prop-types';
import { map } from 'lodash';
import Checkbox from '../Checkbox/Checkbox';
import './Sidebar.scss';

const Sidebar = (props) => {
  const boxes = map(props.items, (name, idx) => (
    <Checkbox key={idx}>{name}</Checkbox>
  ));
  return (
    <div className="sidebar">
      {boxes}
    </div>
  );
}

Sidebar.propTypes = {
  items: PropTypes.arrayOf(PropTypes.string).isRequired,
};

export default Sidebar;
