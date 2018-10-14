import React from 'react';
import Toolbar from '../Toolbar/Toolbar';
import Button from '@material-ui/core/Button';
import './StudyGroup.scss';

class StudyGroup extends React.Component {
  state = {
    sidebarOpen: true,
  }

  toggleSidebar = () => {
    this.setState({ sidebarOpen: !this.state.sidebarOpen });
  }

  render() {
    return (

      <div className="container home">
        <Toolbar toggleSidebar={this.toggleSidebar} />
        <div className="StudyGroup-wrapper">
          <input type="text" placeholder="Zadajte skupinu" />
          <Button
            color="primary"
            variant="contained"
          >
            Odoslat
          </Button>
        </div>

      </div>
    );
  }
}

export default StudyGroup;
