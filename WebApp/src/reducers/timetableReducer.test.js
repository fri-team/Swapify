import reducer, { initState, mergeTimetables } from './timetableReducer';
import {
  LOAD_MY_TIMETABLE,
  LOAD_MY_TIMETABLE_DONE,
  LOAD_MY_TIMETABLE_FAIL,
  LOAD_COURSE_TIMETABLE,
  LOAD_COURSE_TIMETABLE_DONE,
  LOAD_COURSE_TIMETABLE_FAIL,
  SHOW_COURSE_TIMETABLE,
  HIDE_COURSE_TIMETABLE,
} from '../constants/actionTypes';
import loadMyTimetableDone from './__test__/loadMyTimetableDone.json';
import loadCourseTimetableDone from './__test__/loadCourseTimetableDone.json';
import showCourseTimetable from './__test__/showCourseTimetable.json';
import hideCourseTimetable from './__test__/hideCourseTimetable.json';
import mergeTimetablesData from './__test__/mergeTimetables.json';

describe('Reducers: Timetable', () => {
  it('Should return default state on unknown action', () => {
    const action = { type: 'unknownAction' };
    expect(reducer(undefined, action)).toEqual(initState);
  });

  it(`Should set isLoading flag on ${LOAD_MY_TIMETABLE} action`, () => {
    const action = { type: LOAD_MY_TIMETABLE };
    const state = { isLoadingMyTimetable: false };
    const expected = { isLoadingMyTimetable: true };
    expect(reducer(state, action)).toEqual(expected);
  });

  it(`Should unset isLoading flag on ${LOAD_MY_TIMETABLE_FAIL} action`, () => {
    const action = { type: LOAD_MY_TIMETABLE_FAIL };
    const state = { isLoadingMyTimetable: true };
    const expected = { isLoadingMyTimetable: false };
    expect(reducer(state, action)).toEqual(expected);
  });

  it(`Should add myTimetable and myCourses on ${LOAD_MY_TIMETABLE_DONE} action`, () => {
    expect(reducer(loadMyTimetableDone.stateBefore, loadMyTimetableDone.action))
      .toEqual(loadMyTimetableDone.stateAfter);
  });

  it(`Should set isLoading flag on ${LOAD_COURSE_TIMETABLE} action`, () => {
    const action = { type: LOAD_COURSE_TIMETABLE };
    const state = { isLoadingCourse: false };
    const expected = { isLoadingCourse: true };
    expect(reducer(state, action)).toEqual(expected);
  });

  it(`Should unset isLoading flag on ${LOAD_COURSE_TIMETABLE_FAIL} action`, () => {
    const action = { type: LOAD_COURSE_TIMETABLE_FAIL };
    const state = { isLoadingCourse: true };
    const expected = { isLoadingCourse: false };
    expect(reducer(state, action)).toEqual(expected);
  });

  xit(`Should add course name to displayedCourses on ${SHOW_COURSE_TIMETABLE} action`, () => {
    expect(reducer(showCourseTimetable.stateBefore, showCourseTimetable.action))
      .toEqual(showCourseTimetable.stateAfter);
  });

  it(`Should add course timetable to courseTimetables on ${LOAD_COURSE_TIMETABLE_DONE} action`, () => {
    expect(reducer(loadCourseTimetableDone.stateBefore, loadCourseTimetableDone.action))
      .toEqual(loadCourseTimetableDone.stateAfter);
  });

  it(`Should remove course name from displayedCourses on ${HIDE_COURSE_TIMETABLE} action`, () => {
    expect(reducer(hideCourseTimetable.stateBefore, hideCourseTimetable.action))
      .toEqual(hideCourseTimetable.stateAfter);
  });
});

describe('mergeTimetables()', () => {
  it('Should return only myTimetable when second parameter is undefined', () => {
    const myTimetable = [
      {
        day: 1,
        startBlock: 5,
        endBlock: 7,
        courseShortcut: 'TI',
        tearcher: 'Tomáš Majer',
        type: 'laboratory',
      },
    ];
    expect(mergeTimetables(myTimetable)).toEqual(myTimetable);
  });

  it('Should merge myTimetable with course timetables, omitting labs/lectures existing in myTimetable', () => {
    expect(mergeTimetables(mergeTimetablesData.myTimetable, [mergeTimetablesData.courseTimetable]))
      .toEqual(mergeTimetablesData.expected);
  });
});
