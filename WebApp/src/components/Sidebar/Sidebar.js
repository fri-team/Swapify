import React from 'react';
import Drawer from '@material-ui/core/Drawer';
import List from '@material-ui/core/List';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import ListItem from '@material-ui/core/ListItem';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Switch from '@material-ui/core/Switch';
import Typography from '@material-ui/core/Typography';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import CardActions from '@material-ui/core/CardActions';
import Button from '@material-ui/core/Button'
import Grid from '@material-ui/core/Grid';
import DeleteRoundedIcon from '@material-ui/icons/DeleteRounded';

const Sidebar = ({ open, onClose, courses, onCourseToggle,handleChange, value }) => (
  <Drawer open={open} onClose={onClose}>
  <AppBar position="static">
          <Tabs value={value} onChange={handleChange}>
            <Tab label="Predmety" />
            <Tab label="Vymeny" />
          </Tabs>
        </AppBar>
        {value === 0 && 
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
        }
        {value === 1 && 
        <Grid
          container
          direction="column"
          justify="space-around"
          alignItems="stretch"
        >
          <Card>
            <CardContent>
              <Typography variant="h5" component="h2">
                Teoria informacie (TI)
              </Typography>
              <Typography  color="textSecondary" gutterBottom>
                Tomas Majer
              </Typography>
              <Typography  color="textSecondary" gutterBottom>
                Pondelok 11:00
              </Typography>
            </CardContent>
            <CardActions>
              <Button 
                variant="extendedFab" 
                color="primary"
                size="small">
                <DeleteRoundedIcon className="delete" />
                Zrusit
              </Button>
            </CardActions>
          </Card>
          <br />
          <Card>
            <CardContent>
              <Typography variant="h5" component="h2">
                Teoria informacie (TI)
              </Typography>
              <Typography  color="textSecondary" gutterBottom>
                Tomas Majer
              </Typography>
              <Typography  color="textSecondary" gutterBottom>
                Pondelok 15:00
              </Typography>
            </CardContent>
            <CardActions>
              <Button 
                variant="extendedFab" 
                color="primary"
                size="small">
                <DeleteRoundedIcon className="delete" />
                Zrusit
              </Button>
            </CardActions>
          </Card>
          <br />
          <Card>
            <CardContent>
              <Typography variant="h5" component="h2">
                Diskretna simulacia (Diss)
              </Typography>
              <Typography  color="textSecondary" gutterBottom>
                Peter Jankovic
              </Typography>
              <Typography  color="textSecondary" gutterBottom>
                Streda 15:00
              </Typography>
            </CardContent>
            <CardActions>
              <Button 
                variant="extendedFab" 
                color="primary"
                size="small">
                <DeleteRoundedIcon className="delete" />
                Zrusit
              </Button>
            </CardActions>
          </Card>
        </Grid>
      }

  </Drawer>
);

export default Sidebar;