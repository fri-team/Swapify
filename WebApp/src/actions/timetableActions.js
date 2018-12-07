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
  SHOW_EXCHANGE_MODE_TIMETABLE,
  CONFIRM_EXCHANGE_REQUEST,
  CANCEL_EXCHANGE_MODE
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
  var action = {
    type: SHOW_EXCHANGE_MODE_TIMETABLE,
    payload: { course }
  };
  return dowloadCourseTimetableIfNeeded(course.courseName, action);
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

function dowloadCourseTimetableIfNeeded(course, action) {
  return (dispatch, getState) => {
    const { timetable } = getState();
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

export function showCourseTimetable(course) {
  const action = {
    type: SHOW_COURSE_TIMETABLE,
    payload: { course }
  };
  return dowloadCourseTimetableIfNeeded(course, action);
}

export function hideCourseTimetable(course) {
  return {
    type: HIDE_COURSE_TIMETABLE,
    payload: { course }
  };
}

export function cancelExchangeMode(){
  return {
    type: CANCEL_EXCHANGE_MODE
  };
}


export function exchangeConfirm(blockTo) {
  var action = {
    type: CONFIRM_EXCHANGE_REQUEST,
    payload: { blockTo }
  };

  return (dispatch, getState) => {
    const { timetable } = getState();
    var bl = timetable.blockFromExchange;
    const body = {
      BlockFrom:
      {
        "CourseId": bl.courseId,
        "Day": bl.day,
        "StartHour": bl.startBlock,
        "Duration": bl.endBlock - bl.startBlock
      },

      BlockTo: {
        "CourseId": blockTo.courseId,
        "Day": blockTo.day,
        "StartHour": blockTo.startBlock,
        "Duration": blockTo.endBlock - blockTo.startBlock
      },
      //TODO: add real studentID
      StudentId: "00000000-0000-0000-0000-000000000000"
    }

    axios({
      method: 'post',
      url: `/api/exchange/ExchangeConfirm`,
      data: body
    })
      .then(() => {
        dispatch(action);
      })
      .catch(() => {
        dispatch({
          type: CANCEL_EXCHANGE_MODE
        });
      });
  };
}
