/* eslint-disable react/no-find-dom-node */
import React, { PureComponent } from "react";
import ReactDOM from "react-dom";
import styled from "styled-components";
import Button from "@material-ui/core/Button";
import UserAvatar from "./UserAvatar";
import { PullRight, Shaddow } from "./shared";
import DeleteAccountModal from "./DeleteAccount/DeleteAccountModal";
import ClickAwayListener from "@material-ui/core/ClickAwayListener";

const MenuWrapper = styled.div`
  position: fixed;
  top: ${({ y }) => y || 0}px;
  left: ${({ x }) => x || 0}px;
  z-index: 1000;
`;

const Content = styled.div`
  width: ${({ width }) => (width ? `${width}px` : "auto")};
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

const MenuButton = styled(Button)`
  && {
    margin: 3px;
    background-color: #fafafa;
    color: #666;
    border: 1px solid #666;
    box-shadow: none;
    :hover {
      background-color: #fff;
    }
  }
`;

export default class Menu extends PureComponent {
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
    if (document.getElementsByClassName('modal-window').length === 0) {
      this.props.onClose();
    }
  };

  render() {
    const { username, email, onLogout, selectPersonalNumber } = this.props;
    const { x, y } = this.state;
    const width = 300;
    return (
      <ClickAwayListener onClickAway={this.handleClickOutside}>
        <MenuWrapper x={x - width} y={y + 8}>
          <Shaddow>
            <Content width={width}>
              <PadBox>
                <FlexBox>
                  <UserAvatar size={70} username={username} />
                  <div>
                    <div className="username">{username}</div>
                    <div className="email">{email}</div>
                      <DeleteAccountModal 
                      email={email} onLogout={onLogout}/> 
                  </div>
                </FlexBox>
              </PadBox>
              <PadBox>
                <FlexBox>
                  <PullRight />
                  <MenuButton
                    color="primary"
                    variant="contained"
                    onClick={selectPersonalNumber}
                  >
                    Zmeniť číslo
                  </MenuButton>
                  <MenuButton variant="contained" onClick={onLogout}>
                    Odhlásiť
                  </MenuButton>
                </FlexBox>
              </PadBox>
            </Content>
          </Shaddow>
        </MenuWrapper>
      </ClickAwayListener>
    );
  }
}
