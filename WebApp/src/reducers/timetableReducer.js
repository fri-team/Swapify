import {
  concat, differenceWith, flatten, map, merge,
  isEqual, isNil, pick, set, sortBy, uniq, without,
} from 'lodash';
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

export const initState = {
  colHeadings: ['07:00', '08:00', '09:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00', '17:00', '18:00', '19:00'],
  rowHeadings: ['Po', 'Ut', 'St', 'Št', 'Pi'],
  isLoadingMyTimetable: false,
  myTimetable: [],
  myCourseNames: [],
  isLoadingCourse: false,
  courseTimetables: {},
  displayedCourses: [],
  displayedTimetable: [],
};

export default function timetableReducer(state = initState, { type, payload }) {
  switch (type) {
    case LOAD_MY_TIMETABLE:
      return {
        ...state,
        isLoadingMyTimetable: true,
      };
    case LOAD_MY_TIMETABLE_DONE:
      return {
        ...state,
        isLoadingMyTimetable: false,
        myTimetable: payload.timetable,
        myCourseNames: sortBy(uniq(map(payload.timetable, 'courseName'))),
        displayedTimetable: mergeTimetables(payload.timetable, pick(state.courseTimetables, state.displayedCourses)),
      };
    case LOAD_MY_TIMETABLE_FAIL:
      return {
        ...state,
        isLoadingMyTimetable: false,
      };
    case LOAD_COURSE_TIMETABLE:
      return {
        ...state,
        isLoadingCourse: true,
      };
    case LOAD_COURSE_TIMETABLE_DONE:
      return {
        ...state,
        isLoadingCourse: false,
        courseTimetables: merge({}, state.courseTimetables, set({}, payload.course.courseName, payload.course.timetable)),
      };
    case LOAD_COURSE_TIMETABLE_FAIL:
      return {
        ...state,
        isLoadingCourse: false,
      };
    case SHOW_COURSE_TIMETABLE:
      return {
        ...state,
        displayedCourses: concat(state.displayedCourses, payload.course),
        displayedTimetable: mergeTimetables(state.myTimetable, pick(state.courseTimetables, concat(state.displayedCourses, payload.course))),
      };
    case HIDE_COURSE_TIMETABLE:
      return {
        ...state,
        displayedCourses: without(state.displayedCourses, payload.course),
        displayedTimetable: mergeTimetables(state.myTimetable, pick(state.courseTimetables, without(state.displayedCourses, payload.course))),
      };
    default:
      return state;
  }
}

export function mergeTimetables(myTimetable, courseTimetables) {
  if (isNil(courseTimetables)) {
    return myTimetable;
  }
  const coursesWithoutMyLabs = map(courseTimetables, cTimetable => differenceWith(cTimetable, myTimetable, isEqual));
  const myFlaggedTimetable = map(myTimetable, b => merge({}, b, { isMine: true }));
  return concat(myFlaggedTimetable, flatten(coursesWithoutMyLabs));
}
