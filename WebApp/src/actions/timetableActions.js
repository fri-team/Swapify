import axios from 'axios';
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
  SHOW_EXCHANGE_MODE_TIMETABLE
} from '../constants/actionTypes';
import data from './timetableData.json';

export function loadMyTimetable() {
  return dispatch => {
    dispatch({
      type: LOAD_MY_TIMETABLE
    });
    axios({
      method: 'get',
      url: '/api/timetable'
    })
      .then(res => {
        dispatch({
          type: LOAD_MY_TIMETABLE_DONE,
          payload: {
            timetable: res.data.blocks
          }
        });
      })
      .catch(() => {
        dispatch({
          type: LOAD_MY_TIMETABLE_FAIL
        });
        // fallback if API is not running, TODO: remove in the future
        dispatch({
          type: LOAD_MY_TIMETABLE_DONE,
          payload: {
            timetable: data.timetable
          }
        });
      });
  };
}

export function showExchangeModeTimetable(course) {
  return dispatch => {
    dispatch({
      type: SHOW_EXCHANGE_MODE_TIMETABLE,
      payload: { course }
    });
  };
}

function loadCourseTimetableAsync(dispatch, course) {
  return new Promise(resolve => {
    setTimeout(() => {
      dispatch({
        type: LOAD_COURSE_TIMETABLE_DONE,
        payload: {
          course: {
            courseName: course,
            timetable: data.courses[course]
          }
        }
      });
      resolve();
    }, 100);
  });
}

export function loadCourseTimetable(course) {
  return dispatch => {
    dispatch({
      type: LOAD_COURSE_TIMETABLE
    });
    loadCourseTimetableAsync(dispatch, course);
  };
}

export function showCourseTimetable(course) {
  return (dispatch, getState) => {
    const { timetable } = getState();
    const action = {
      type: SHOW_COURSE_TIMETABLE,
      payload: { course }
    };
    if (!_.has(timetable.courseTimetables, course)) {
      axios({
        method: 'get',
        url: `/api/timetable/course/${course}`
      })
        .then(res => {
          dispatch({
            type: LOAD_COURSE_TIMETABLE_DONE,
            payload: {
              course: {
                courseName: course,
                timetable: res.data.blocks
              }
            }
          });
        })
        .catch(() => {
          dispatch({
            type: LOAD_COURSE_TIMETABLE_FAIL
          });
          // fallback, TODO: modify logic to return data from API
          loadCourseTimetableAsync(dispatch, course).then(() => {
            dispatch(action);
          });
        });
    } else {
      dispatch(action);
    }
  };
}

export function hideCourseTimetable(course) {
  return {
    type: HIDE_COURSE_TIMETABLE,
    payload: { course }
  };
}
