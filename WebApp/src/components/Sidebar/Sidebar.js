import React from 'react';
import Drawer from '@material-ui/core/Drawer';
import List from '@material-ui/core/List';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import ListItem from '@material-ui/core/ListItem';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Switch from '@material-ui/core/Switch';
import Card from '@material-ui/core/Card';
import Grid from '@material-ui/core/Grid';

import IconButton from '@material-ui/core/IconButton';
import DeleteIcon from '@material-ui/icons/Delete';
import { CardHeader } from '@material-ui/core';

const Sidebar = ({ open, onClose, courses, onCourseToggle, handleChange, value, exchangeRequests }) => (
  <Drawer open={open} onClose={onClose}>
    <AppBar position="static">
      <Tabs value={value} onChange={handleChange}>
        <Tab label="Predmety" />
        <Tab label="Vymeny" />
      </Tabs>
    </AppBar>
    {value === 0 &&
      <List>
        {courses.map(({ courseId, courseName, checked }) => (
          <ListItem button key={courseId}>
            <FormControlLabel
              control={
                <Switch
                  checked={checked}
                  onChange={(_, checked) => onCourseToggle(courseId, courseName, checked)}
                />
              }
              label={courseName}
            />
          </ListItem>

        ))}
      </List>
    }
    {value === 1 &&
      <Grid
        container
        direction="column"
        justify="space-around"
        alignItems="stretch"
      >
        {exchangeRequests.map(({ dateOfCreation }) => (
          <Card>
            <CardHeader
              action={
                <IconButton>
                  <DeleteIcon />
                </IconButton>
              }
              title="Teoria informacie (TI)"
              subheader={dateOfCreation}
            />
          </Card>
        ))}
      </Grid>
    }

  </Drawer>
);

export default Sidebar;