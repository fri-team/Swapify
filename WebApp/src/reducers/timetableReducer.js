import _ from 'lodash';
import {
  LOAD_MY_TIMETABLE,
  LOAD_MY_TIMETABLE_DONE,
  LOAD_MY_TIMETABLE_FAIL,
  LOAD_COURSE_TIMETABLE,
  LOAD_COURSE_TIMETABLE_DONE,
  LOAD_COURSE_TIMETABLE_FAIL,
  SHOW_COURSE_TIMETABLE,
  HIDE_COURSE_TIMETABLE,
  SHOW_EXCHANGE_MODE_TIMETABLE,
  REMOVE_BLOCK,
  REMOVE_BLOCK_DONE,
  REMOVE_BLOCK_FAIL
} from '../constants/actionTypes';

export const initState = {
  colHeadings: [
    '07:00',
    '08:00',
    '09:00',
    '10:00',
    '11:00',
    '12:00',
    '13:00',
    '14:00',
    '15:00',
    '16:00',
    '17:00',
    '18:00',
    '19:00'
  ],
  rowHeadings: ['Po', 'Ut', 'St', 'Št', 'Pi'],
  isLoadingMyTimetable: false,
  myTimetable: [],
  myCourseNames: [],
  isLoadingCourse: false,
  courseTimetables: {},
  displayedCourses: [],
  displayedTimetable: [],
  isExchangeMode: false,
  isBlockRemoved: false,
};

export default function timetableReducer(state = initState, { type, payload }) {
  switch (type) {
    case LOAD_MY_TIMETABLE:
      return {
        ...state,
        isLoadingMyTimetable: true
      };
    case LOAD_MY_TIMETABLE_DONE:
      return {
        ...state,
        isLoadingMyTimetable: false,
        myTimetable: payload.timetable,
        myCourseNames: _.sortBy(_.uniq(_.map(payload.timetable, 'courseName'))),
        displayedTimetable: mergeTimetables(
          payload.timetable,
          _.pick(state.courseTimetables, state.displayedCourses)
        )
      };
    case LOAD_MY_TIMETABLE_FAIL:
      return {
        ...state,
        isLoadingMyTimetable: false
      };
    case LOAD_COURSE_TIMETABLE:
      return {
        ...state,
        isLoadingCourse: true
      };
    case LOAD_COURSE_TIMETABLE_DONE:
      return {
        ...state,
        isLoadingCourse: false,
        courseTimetables: _.merge(
          {},
          state.courseTimetables,
          _.set({}, payload.course.courseName, payload.course.timetable)
        )
      };
    case LOAD_COURSE_TIMETABLE_FAIL:
      return {
        ...state,
        isLoadingCourse: false
      };
    case SHOW_COURSE_TIMETABLE:
      return {
        ...state,
        displayedCourses: _.concat(state.displayedCourses, payload.course),
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(
            state.courseTimetables,
            _.concat(state.displayedCourses, payload.course)
          )
        )
      };
    case SHOW_EXCHANGE_MODE_TIMETABLE:
      return {
        ...state,
        isExchangeMode: true,
        displayedTimetable: _.differenceWith(state.courseTimetables[payload.course.courseName],state.myTimetable, _.isEqual)
      };
    case HIDE_COURSE_TIMETABLE:
      return {
        ...state,
        displayedCourses: _.without(state.displayedCourses, payload.course),
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(
            state.courseTimetables,
            _.without(state.displayedCourses, payload.course)
          )
        )
      };
    case REMOVE_BLOCK:
    return {
      ...state,
      displayedTimetable: mergeTimetables(
        state.myTimetable,
        _.pick(state.courseTimetables, state.displayedCourses)
      ),
      isBlockRemoved: false
    };
    case REMOVE_BLOCK_DONE:
    return {
      ...state,
      displayedTimetable: mergeTimetables(
        state.myTimetable,
        _.pick(state.courseTimetables, state.displayedCourses)
      ),
      isBlockRemoved: true
    };
    case REMOVE_BLOCK_FAIL:
    return {
      ...state,
      displayedTimetable: mergeTimetables(
        state.myTimetable,
        _.pick(state.courseTimetables, state.displayedCourses)
      ),
      isBlockRemoved: false
    };
    default:
      return state;
  }
}

export function mergeTimetables(myTimetable, courseTimetables) {
  if (_.isNil(courseTimetables)) {
    return myTimetable;
  }
  const coursesWithoutMyLabs = _.map(courseTimetables, cTimetable =>
    _.differenceWith(cTimetable, myTimetable, _.isEqual)
  );
  const myFlaggedTimetable = _.map(myTimetable, b =>
    _.merge({}, b, { isMine: true })
  );
  return _.concat(myFlaggedTimetable, _.flatten(coursesWithoutMyLabs));
}
