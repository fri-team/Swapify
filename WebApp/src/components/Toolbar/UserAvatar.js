import React, { PureComponent } from 'react';
import styled, { css } from 'styled-components';
import Avatar from '@material-ui/core/Avatar';

const UserAvatar = styled(Avatar)`
  && {
    color: ${({ color }) => color || '#fff'};
    background-color: ${({ theme, background }) =>
      background || theme.color.secondary};
    ${({ size }) =>
      size &&
      css`
        width: ${size}px;
        height: ${size}px;
        font-size: calc(${size}px * 0.6);
      `}
  }
`;

export default class UserAvatarWrapper extends PureComponent {
  render() {
    const { username, ...restProps } = this.props;
    return <UserAvatar {...restProps}>{username[0].toUpperCase()}</UserAvatar>;
  }
}
