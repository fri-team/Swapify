/* eslint-disable react/no-find-dom-node */
import React, { PureComponent } from 'react';
import ReactDOM from 'react-dom';
import styled from 'styled-components';
import onClickOutside from 'react-onclickoutside';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import { PullRight, Shaddow } from '../Toolbar/shared';
import NavigationIcon from '@material-ui/icons/Navigation';
import Radio from '@material-ui/core/Radio';
import RadioGroup from '@material-ui/core/RadioGroup';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import axios from 'axios';
import { TIMETABLE } from '../../util/routes';

const MenuWrapper = styled.div`
  position: absolute;
  z-index: 1000;
`;

const Content = styled.div`
  width: ${({ width }) => (width ? `${width}px` : 'auto')};
  border-radius: 2px;
  border-color: 1px solid #e0e0e0;
  color: #000;
  background-color: #fff;
  > * + * {
    border-top: 1px solid #e0e0e0;
  }
  .username {
    font-weight: 600;
  }
  .email {
    font-size: 0.7em;
  }
`;

const PadBox = styled.div`
  padding: 1em;
`;

const FlexBox = styled.div`
  display: flex;
  align-items: center;
  > * + * {
    margin-left: 1em;
  }
`;

class AddBlockForm extends PureComponent {
  state = {
    x: 0,
    y: 0,
    Day : this.props.day,
    CourseName : '',
    Teacher: '',
    Room: '',
    StartBlock: this.props.start,
    Length: 2,
    Type: '',
    user: this.props.user,
    CourseShortcut: ''
  };

  componentDidMount() {
    this.calcPosition();
  }

  componentDidUpdate() {
    this.calcPosition();
  }

  calcPosition = () => {
    const ref = this.props.renderRef;
    if (!ref) return;
    const element = ReactDOM.findDOMNode(ref);
    const rect = element && element.getBoundingClientRect();
    const position = { x: rect.right, y: rect.bottom };
    const { x, y,  } = this.state;
    if (position.x != x || position.y != y) this.setState(position);
  };

  handleClickOutside = () => {
    this.props.onClose();
  };

  decodeDay = (day) => {
    switch (day) {
    case 1 :
      return "Pondelok";
    case 2: 
      return "Utorok";
    case 3: 
      return "Streda";
    case 4: 
      return "Štvrtok";
    case 5: 
      return "Piatok";
    }
  }

  SubmitBlockChange = () => {
    const block = {
      Day : this.props.day,
      CourseName : this.state.CourseName,
      CourseShortcut: this.state.CourseShortcut,
      Teacher: this.state.Teacher,
      Room: this.state.Room,
      StartBlock: this.props.start,
      EndBlock: this.props.start + this.state.Length,
      Type: this.state.Type
    }
    const body = {
      user: this.state.user,
      block: block
    }
    
    
    axios({
      method: 'post',
      url: '/api/student/addNewBlock',
      data: body
    })
      .then(() => {
        this.props.history.push(TIMETABLE);
      });
    
  }

  handleCourse = (evt) => {
    this.setState({ CourseName: evt.target.value });
    if (this.state.CourseName.length > 2) {
      axios({
        method: 'get',
        url: '/api/timetable/course/getCoursesAutoComplete/' + evt.target.value
      })
      .then(() => {
      })
      .catch(() => {
      });
    }
  }

  handleTeacher = (evt) => {
    this.setState({ Teacher: evt.target.value });
  }

  handleRoom = (evt) => {
    this.setState({ Room: evt.target.value });
  }

  handleLenght = (evt) => {
    this.setState({ Length: evt.target.value });
  }

  handleType = (evt) => {
    this.setState({ Type: evt.target.value });
  }

  handleChange = event => {
    this.setState({ value: event.target.value });
  };

  handleShortcut = event => {
    this.setState({ value: event.target.value });
  };

  canBeSubmitted = () => {
    return this.state.Length !== '' && 
           this.state.CourseName !== '' && 
           this.state.Teacher !== '' &&
           this.state.Room !== '' && 
           this.state.Type !== '' &&
           this.state.CourseShortcut !== '';
  }

  render() {
    const { x, y } = this.state;
    const width = 400;
    return (
      <MenuWrapper x={x - width} y={y + 8}>
        <Shaddow>
          <Content width={width}>
            <PadBox>
              <FlexBox>
                <form>
              <TextField
                id="CourseName"
                label="Predmet"
                placeholder="Zadajte nazov predmetu"
                onChange={this.handleCourse}
                value={this.state.CourseName}
                margin="normal"
                fullWidth
              />
               <TextField
                id="CourseShorcut"
                label="Skratka predmetu"
                placeholder="Zadajte skratku predmetu"
                onChange={this.handleShortcut}
                value={this.state.CourseShortcut}
                margin="normal"
                fullWidth
              />
              <TextField
                id="Teacher"
                label="Profesor"
                placeholder="Meno profesora"
                margin="normal"
                onChange={this.handleTeacher}
                value={this.state.Teacher}
                fullWidth
              />
              <TextField
                id="Room"
                label="Miestnost"
                placeholder="Miestnost"
                onChange={this.handleRoom}
                value={this.state.Room}
                margin="normal"
                fullWidth
              />
              
              <TextField
                id="Day"
                label="Deň"
                value={this.decodeDay(this.state.Day)}
                margin="normal"
                disabled
                fullWidth
              />              
              <TextField
                id="StartBlock"
                label="Začiatok"
                value={this.state.StartBlock + ":00"}
                margin="normal"
                disabled
                fullWidth
              />
               <TextField
                id="length"
                label="Dlzka"
                value={this.state.Length}
                onChange={this.handleLenght}
                margin="normal"
                fullWidth
              />

              <RadioGroup
                aria-label="Type"
                name="Type"
                value={this.state.Type}
                onChange={this.handleType}
              >
                <FormControlLabel value="Lecture" control={<Radio />} label="Prednaska" />
                <FormControlLabel value="Laboratory" control={<Radio />} label="Laboratorium" />
                <FormControlLabel value="Exercise" control={<Radio />} label="Cvicenie" />
                
                
              </RadioGroup>
              <PullRight>
              <Button 
                variant="extendedFab" 
                color="primary" 
                className="save"
                onClick={this.SubmitBlockChange} 
                disabled={!this.canBeSubmitted()}
              >
                <NavigationIcon className="save" />
                 Ulozit
              </Button>
              </PullRight>
              </form>
              </FlexBox>
            </PadBox>
          </Content>
        </Shaddow>
      </MenuWrapper>
    );
  }
}


export default onClickOutside(AddBlockForm);
