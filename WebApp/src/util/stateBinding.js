import { merge, set, isFunction } from 'lodash';

export function bind(stateAttribute, component, execBefore) {
  return (event) => {
    if (isFunction(execBefore)) {
      execBefore(event);
    }
    const newState = merge({}, component.state, set({}, stateAttribute, event.target.value));
    component.setState(newState);
  };
}
