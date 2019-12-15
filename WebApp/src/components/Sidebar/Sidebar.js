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
import MailIcon from '@material-ui/icons/Mail';
import { dayHourToString } from '../../util/dateTimeFormatter';
import { CardHeader, Button } from '@material-ui/core';

import './Sidebar.scss';

const Sidebar = ({ open, onClose, courses, onCourseToggle, toggleMailUsModalWindow, handleChange, value, exchangeRequests }) => (
  <Drawer open={open} onClose={onClose}>
    <AppBar position="static">
      <Tabs value={value} onChange={handleChange}>
        <Tab label="Predmety" />
        <Tab label="Vymeny" />
      </Tabs>
    </AppBar>
    <div className="drawerWrapper">
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
          {createExchangeRequestsList(exchangeRequests, courses)}
        </Grid>
      }
      
      <Button onClick={toggleMailUsModalWindow}>
        <MailIcon /> &nbsp; Napíšte nám
      </Button>
    </div>
  </Drawer>
);

const createExchangeRequestsList = (exchangeRequests, courses) => {
  return exchangeRequests.map((exchangeRequest) => createExchangeRequestListItem(exchangeRequest, courses));
}

const createExchangeRequestListItem = (exchangeRequest, courses) => {
  var course = courses.find(course => course.courseId == exchangeRequest.blockFrom.courseId)

  if (course == null) {
    return null;
  }
  return (
    <Card>
      <CardHeader        
        title={course.courseName}
        subheader={dayHourToString(exchangeRequest.blockFrom.day, exchangeRequest.blockFrom.startHour)
          + " -> " + dayHourToString(exchangeRequest.blockTo.day, exchangeRequest.blockTo.startHour)}
      />
    </Card>
  )
}

export default Sidebar;