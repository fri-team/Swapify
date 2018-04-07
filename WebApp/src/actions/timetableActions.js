import { has } from 'lodash';
import {
  LOAD_MY_TIMETABLE,
  LOAD_MY_TIMETABLE_DONE,
  // LOAD_MY_TIMETABLE_FAIL,
  LOAD_COURSE_TIMETABLE,
  LOAD_COURSE_TIMETABLE_DONE,
  // LOAD_COURSE_TIMETABLE_FAIL,
  SHOW_COURSE_TIMETABLE,
  HIDE_COURSE_TIMETABLE,
} from '../constants/actionTypes';
import data from './timetableData.json';

export function loadMyTimetable() {
  return (dispath) => {
    dispath({
      type: LOAD_MY_TIMETABLE,
    });
    setTimeout(() => {
      dispath({
        type: LOAD_MY_TIMETABLE_DONE,
        payload: {
          timetable: data.timetable,
        },
      });
    }, 1000);
  };
}

function loadCourseTimetableAsync(dispath, course) {
  return new Promise((resolve) => {
    setTimeout(() => {
      dispath({
        type: LOAD_COURSE_TIMETABLE_DONE,
        payload: {
          course: {
            courseName: course,
            timetable: data.courses[course],
          },
        },
      });
      resolve();
    }, 100);
  });
}

export function loadCourseTimetable(course) {
  return (dispath) => {
    dispath({
      type: LOAD_COURSE_TIMETABLE,
    });
    loadCourseTimetableAsync(dispath, course);
  };
}

export function showCourseTimetable(course) {
  return (dispath, getState) => {
    const { timetable } = getState();
    const action = {
      type: SHOW_COURSE_TIMETABLE,
      payload: { course },
    };
    if (!has(timetable.courseTimetables, course)) {
      loadCourseTimetableAsync(dispath, course)
        .then(() => {
          dispath(action);
        });
    } else {
      dispath(action);
    }
  };
}

export function hideCourseTimetable(course) {
  return {
    type: HIDE_COURSE_TIMETABLE,
    payload: { course },
  };
}
