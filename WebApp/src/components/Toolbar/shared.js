import styled from 'styled-components';

export const PullRight = styled.div`
  margin-left: auto;
`;

export const Shaddow = styled.div`
  box-shadow: ${({ size }) => {
    const map = {
      small: 2,
      medium: 4,
      large: 8
    };
    const s = map[size] || map.medium;
    return `${s}px ${s}px 16px ${s}px rgba(0, 0, 0, 0.36)`;
  }};
`;
