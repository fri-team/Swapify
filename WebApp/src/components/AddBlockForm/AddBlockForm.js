/* eslint-disable react/no-find-dom-node */
import React, { PureComponent } from 'react';
import ReactDOM from 'react-dom';
import styled from 'styled-components';
import onClickOutside from 'react-onclickoutside';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import { PullRight, Shaddow } from '../Toolbar/shared';
import NavigationIcon from '@material-ui/icons/Navigation';

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
  state = { x: 0, y: 0 };

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
    const { x, y } = this.state;
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

  render() {
    const { start, day } = this.props;
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
                id="subject"
                label="Predmet"
                placeholder="Zadajte nazov predmetu"
                margin="normal"
                fullWidth
              />
              <TextField
                id="teacher"
                label="Profesor"
                placeholder="Meno profesora"
                margin="normal"
                fullWidth
              />
              <TextField
                id="day"
                label="Deň"
                defaultValue={this.decodeDay(day)}
                margin="normal"
                disabled
                fullWidth
              />
              <TextField
                id="start"
                label="Začiatok"
                defaultValue={start + ":00"}
                margin="normal"
                disabled
                fullWidth
              />
               <TextField
                id="length"
                label="Dlzka"
                defaultValue="2"
                margin="normal"
                fullWidth
              />
              <PullRight>
              <Button variant="extendedFab" color="primary" className="save">
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
