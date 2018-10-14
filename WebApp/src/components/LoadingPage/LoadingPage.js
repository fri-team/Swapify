import React from 'react';
import CircularProgress from '@material-ui/core/CircularProgress';

const style = {
  width: '100%',
  height: '100%',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center'
};

const LoadingPage = () => {
  console.log('loading');
  return (
    <div style={style}>
      <CircularProgress size={50} />
    </div>
  );
};

export default LoadingPage;
