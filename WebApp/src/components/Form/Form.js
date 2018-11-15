import React from 'react';

const Form = ({ children, onSubmit, ...restProps }) => (
  <form
    {...restProps}
    onSubmit={e => {
      e.preventDefault();
      if (onSubmit) onSubmit();
    }}
  >
    {children}
  </form>
);

export default Form;
