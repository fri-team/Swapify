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
  EDIT_BLOCK,
  EDIT_BLOCK_DONE,
  EDIT_BLOCK_FAIL,
  CONFIRM_EXCHANGE_REQUEST,
  CANCEL_EXCHANGE_MODE,
  ADD_BLOCK,
  ADD_BLOCK_DONE,
  ADD_BLOCK_FAIL,
  CHOOSE_EXCHANGE_FROM_BLOCK
} from '../constants/actionTypes';
import data from './timetableData.json';
import { loadExchangeRequests } from './exchangeActions';
import { blockNumberToHour } from '../util/convertFunctions';

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
        window.location.replace("http://localhost:3000/personal-number");
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
  var courseId = course.courseId;
  const action = {
    type: SHOW_COURSE_TIMETABLE,
    payload: { courseId }
  };
  return dowloadCourseTimetableIfNeeded(course.courseId, course.courseName, action);
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

export function hideCourseTimetable(courseId = null) {
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
        blockId: bl.id,
        courseId: bl.courseId,
        day: bl.day,
        startHour: blockNumberToHour(bl.startBlock),
        duration: bl.endBlock - bl.startBlock
      },

      BlockTo: {
        blockId: blockTo.id,
        courseId: blockTo.courseId,
        day: blockTo.day,
        startHour: blockNumberToHour(blockTo.startBlock),
        duration: blockTo.endBlock - blockTo.startBlock
      },
      StudentId: user.studentId
    }    

    axios({
      method: 'post',
      url: `/api/exchange/exchangeConfirm`,
      data: body
    })
      .then((response) => { 
        var exchangeMade = response.data;
        if (exchangeMade === false) {
          window.alert("Žiadosť o výmenu bola evidovaná.");          
        } else {          
          window.alert("Výmena bola vykonaná.");  
          dispatch(loadMyTimetable(user.email));
        }
        dispatch(hideCourseTimetable(bl.id));
        dispatch(action);        
        dispatch(loadExchangeRequests());
      })
      .catch(() => {
        window.alert("Pri vytváraní žiadosti nastala chyba.");
        dispatch(hideCourseTimetable(bl.id));
        dispatch({
          type: CANCEL_EXCHANGE_MODE
        });        
      });
  };
}

export function removeBlock(body, userEmail) {
  return dispatch => {
    dispatch({
      type: REMOVE_BLOCK
    });
    axios({
      method: 'delete',
      url: `/api/student/removeBlock/${userEmail}/${body.id}`
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

export function editBlock(body, userEmail) {
  return dispatch => {
    dispatch({
      type: EDIT_BLOCK
    });
    axios({
      method: 'put',
      url: `/api/student/editBlock`,
      data: body
    })
    .then(() =>{
      dispatch({
        type: EDIT_BLOCK_DONE
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
        type: EDIT_BLOCK_FAIL
      });
    });
  };
}
