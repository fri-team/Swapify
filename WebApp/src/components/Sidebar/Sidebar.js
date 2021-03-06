import React from 'react';
import Drawer from '@material-ui/core/Drawer';
import List from '@material-ui/core/List';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import ListItem from '@material-ui/core/ListItem';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Tooltip from '@material-ui/core/Tooltip';
import Switch from '@material-ui/core/Switch';
import Card from '@material-ui/core/Card';
import Grid from '@material-ui/core/Grid';
import { dayHourToString } from '../../util/dateTimeFormatter';
import { CardHeader } from '@material-ui/core';
import { withStyles } from '@material-ui/core/styles';
import IconButton from '@material-ui/core/IconButton';
import AddIcon from '@material-ui/icons/Add';
//import SidebarForm from '../Sidebar/SidebarForm';
import SideBarForm from './SideBarForm';

import './Sidebar.scss';


const StyledTab= withStyles({
  root: {
    width: 200
  }
})(Tab);

const Sidebar = ({ open, onClose, courses, onCourseToggle, handleChange, value, exchangeRequests, addClickHandle, sideBarFormOpen,onCloseForm}) => (
  <Drawer open={open} onClose={onClose}>
    <AppBar position="static">
      <Tabs value={value} onChange={handleChange}>
        <StyledTab label="Predmety" />
        <StyledTab label="Výmeny" />
      </Tabs>
    </AppBar>
    <div className="drawerWrapper">
    {value === 0 &&                
        <List> 
          <Tooltip title="Pridať predmet" placement="top">
            <IconButton onClick={addClickHandle}><AddIcon/></IconButton>
          </Tooltip>
          {courses.map(({ courseId, courseName, checked }) => (
            <ListItem button key={courseId}>
              <FormControlLabel
                control={
                  <Tooltip title="Zobraziť rozvrh predmetu" placement="top">
                    <Switch
                      checked={checked}
                      onChange={(_, checked) => onCourseToggle(courseId, courseName, checked)}
                    />
                  </Tooltip>
                }
                label={courseName}
              />
            </ListItem>

          ))}
          {sideBarFormOpen && 
            <SideBarForm
              onClose={onCloseForm}           
            />
          }        
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
