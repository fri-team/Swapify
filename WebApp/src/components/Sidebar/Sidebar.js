import React from 'react';
import Drawer from '@material-ui/core/Drawer';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Switch from '@material-ui/core/Switch';

const Sidebar = ({ open, onClose, courses, onCourseToggle }) => (
  <Drawer open={open} onClose={onClose}>
    <List>
      {courses.map(({ name, checked }) => (
        <ListItem button key={name}>
          <FormControlLabel
            control={
              <Switch
                checked={checked}
                onChange={(_, checked) => onCourseToggle(name, checked)}
              />
            }
            label={name}
          />
        </ListItem>
      ))}
    </List>
  </Drawer>
);

export default Sidebar;
