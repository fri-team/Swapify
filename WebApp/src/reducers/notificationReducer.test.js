import reducer from './notificationReducer';
import notificationsFetchDone from './__test__/notificationsFetchDone';
import notificationReadChanged from './__test__/notificationReadChanged';

describe('Reducers: notificationReducer.', () => {
    it('Should save loaded notifications.', () => {          
        expect(reducer(notificationsFetchDone.stateBefore, notificationsFetchDone.action))
            .toEqual(notificationsFetchDone.stateAfter);
    });  
    it('Should change read state of notification.', () => {          
        expect(reducer(notificationReadChanged.stateBefore, notificationReadChanged.action))
            .toEqual(notificationReadChanged.stateAfter);
  });  
});
