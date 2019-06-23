import reducer from './exchangeRequestsReducer';
import loadExchangeRequestsDone from './__test__/loadExchangeRequestsDone';


describe('Reducers: exchangeRequestReducer', () => {
    it('Should save loaded exchange requests', () => {          
        expect(reducer(loadExchangeRequestsDone.stateBefore, loadExchangeRequestsDone.action))
            .toEqual(loadExchangeRequestsDone.stateAfter);
  });  
});
