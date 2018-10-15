import React from 'react';
import Toolbar from '../Toolbar/Toolbar';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import SaveIcon from '@material-ui/icons/Save';
import './StudyGroup.scss';

class StudyGroup extends React.Component {
  state = {
    sidebarOpen: true,
  }

  render() {
    return (
      <div className="container home">
        <Toolbar toggleSidebar={this.toggleSidebar} />
        <div className="StudyGroup-wrapper">
            <TextField
                label="Zadajte skupinu"
                placeholder="Cislo skupiny"
                margin="normal"
                multiline={true}
                fullWidth={true}
             />
            <br />
            <br />
             <Button
                color="primary"
                variant="fab"
                mini
                className="fab">
               <SaveIcon/>
             </Button>
        </div>
      </div>
    );
  }
}

export default StudyGroup;
