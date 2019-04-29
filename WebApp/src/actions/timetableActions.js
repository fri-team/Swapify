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
  REMOVE_BLOCK,
  REMOVE_BLOCK_DONE,
  REMOVE_BLOCK_FAIL,
  CONFIRM_EXCHANGE_REQUEST,
  CANCEL_EXCHANGE_MODE,
  ADD_BLOCK,
  ADD_BLOCK_DONE,
  ADD_BLOCK_FAIL,
  CHOOSE_EXCHANGE_FROM_BLOCK
} from '../constants/actionTypes';
import data from './timetableData.json';

export function loadMyTimetable(userEmail) {
  return dispatch => {
    dispatch({
      type: LOAD_MY_TIMETABLE
    });
    axios({
      method: 'get',
      url: '/api/student/getStudentTimetable/' + userEmail
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
        window.location.replace("http://localhost:3000/study-group");
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
  var courseId = course.id;
  const action = {
    type: SHOW_COURSE_TIMETABLE,
    payload: { courseId }
  };
  return dowloadCourseTimetableIfNeeded(course.id, course.courseName, action);
}

function loadCourseTimetableAsync(dispatch, id, name) {
  return new Promise(resolve => {
    setTimeout(() => {
      dispatch({
        type: LOAD_COURSE_TIMETABLE_DONE,
        payload: {
          course: {
            courseId: id,
            courseName: name,
            timetable: data.courses[id]
          }
        }
      });
      resolve();
    }, 100);
  });
}

export function loadCourseTimetable(courseId, courseName) {
  return dispatch => {
    dispatch({
      type: LOAD_COURSE_TIMETABLE
    });
    loadCourseTimetableAsync(dispatch, courseId, courseName);
  };
}

function dowloadCourseTimetableIfNeeded(id, name, action) {
  return (dispatch, getState) => {
    const { timetable } = getState();
    if (!_.has(timetable.courseTimetables, id)) {
      axios({
        method: 'get',
        url: `/api/timetable/getCourseTimetable/${id}`
      })
        .then(res => {
          dispatch({
            type: LOAD_COURSE_TIMETABLE_DONE,
            payload: {
              course: {
                courseId: id,
                courseName: name,
                timetable: res.data.blocks
              }
            }
          });
          dispatch(action);
        })
        .catch(() => {
          dispatch({
            type: LOAD_COURSE_TIMETABLE_FAIL
          });
          // fallback, TODO: modify logic to return data from API
          loadCourseTimetableAsync(dispatch, id, name).then(() => {
            dispatch(action);
          });
        });
    } else {
      dispatch(action);
    }
  };
}

export function showCourseTimetable(courseId, courseName) {
  const action = {
    type: SHOW_COURSE_TIMETABLE,
    payload: { courseId }
  };
  return dowloadCourseTimetableIfNeeded(courseId, courseName, action);
}

export function hideCourseTimetable(courseId) {
  return {
    type: HIDE_COURSE_TIMETABLE,
    payload: { courseId }
  };
}

export function cancelExchangeMode(){
  return {
    type: CANCEL_EXCHANGE_MODE
  };
}

export function chooseExchangeFromBlock(course) {
  return {
    type: CHOOSE_EXCHANGE_FROM_BLOCK,
    payload: {
      course
    }
  }  
}

export function exchangeConfirm(blockTo) {
  var action = {
    type: CONFIRM_EXCHANGE_REQUEST,
    payload: { blockTo }
  };

  return (dispatch, getState) => {
    const { timetable, user } = getState();
    var bl = timetable.blockFromExchange;
    const body = {
      BlockFrom:
      {
        courseId: bl.id,
        day: bl.day,
        startHour: bl.startBlock,
        duration: bl.endBlock - bl.startBlock
      },

      BlockTo: {
        courseId: blockTo.id,
        day: blockTo.day,
        startHour: blockTo.startBlock,
        duration: blockTo.endBlock - blockTo.startBlock
      },
      StudentId: user.studentId
    }    

    axios({
      method: 'post',
      url: `/api/exchange/ExchangeConfirm`,
      data: body
    })
      .then(() => {
        //TODO hide course timetable    
        dispatch(hideCourseTimetable(blockTo.id));
        dispatch(action);            
      })
      .catch(() => {
        dispatch(hideCourseTimetable(blockTo.id));
        dispatch({
          type: CANCEL_EXCHANGE_MODE
        });        
      });
  };
}

export function removeBlock(course, userEmail) {
  const block = {
    day: course.day,
    teacher: (course.teacher === '') ? null : course.teacher,
    room: (course.room === '') ? null : course.room,
    startHour: course.startBlock + 6,
    duration: course.endBlock - course.startBlock,
    type: ((course.type == 'laboratory') ? (2) : (3))
  }

  return dispatch => {
    dispatch({
      type: REMOVE_BLOCK
    });
    axios({
      method: 'delete',
      url: `/api/student/${userEmail}/blocks/${block.day}/${block.teacher}/${block.room}/${block.startHour}/${block.duration}/${block.type}`
    })
    .then(() =>{
      dispatch({
        type: REMOVE_BLOCK_DONE
      });
      axios({
        method: 'get',
        url: '/api/student/getStudentTimetable/' + userEmail
      })
        .then(res => {
          dispatch({
            type: LOAD_MY_TIMETABLE_DONE,
            payload: {
              timetable: res.data.blocks
            }
          });
        })
    })
    .catch(() => {
      window.alert('Nepodarilo sa vymazať blok, skúste to neskôr prosím.');
      dispatch({
        type: REMOVE_BLOCK_FAIL
      });
    });
  };
}

export function addBlock(body, userEmail) {
  return dispatch => {
    dispatch({
      type: ADD_BLOCK
    });
    axios({
      method: 'post',
      url: `/api/student/addNewBlock`,
      data: body
    })
    .then(() =>{
      dispatch({
        type: ADD_BLOCK_DONE
      });
      axios({
        method: 'get',
        url: '/api/student/getStudentTimetable/' + userEmail
      })
        .then(res => {
          dispatch({
            type: LOAD_MY_TIMETABLE_DONE,
            payload: {
              timetable: res.data.blocks
            }
          });
        })
    })
    .catch(() => {
      window.alert('Nepodarilo sa pridat blok, skúste to neskôr prosím.');
      dispatch({
        type: ADD_BLOCK_FAIL
      });
    });
  };
}
