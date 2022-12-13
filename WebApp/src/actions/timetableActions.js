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
import { PERSONALNUMBER } from '../util/routes';

export function loadMyTimetable(user, history) {
  return dispatch => {
    dispatch({
      type: LOAD_MY_TIMETABLE
    });
    if (user.personalNumber == null) {
      history.push(PERSONALNUMBER);
      if (user.firstTimePN != null) {
        user.personalNumber = user.firstTimePN;
      }
    }
    axios({
      method: 'get',
      url: '/api/timetabledata/getUserTimetable/' + user.email
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
    const { timetable, user } = getState();
    if (!_.has(timetable.courseTimetables, id)) {
      axios({
        method: 'get',
        url: `/api/timetable/getCourseTimetable/${id}/${user.studentId}`
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
      BlockFrom: {
        blockId: bl.id,
        courseId: bl.courseId,
        day: bl.day,
        startHour: blockNumberToHour(bl.startBlock),
        duration: bl.endBlock - bl.startBlock,
        room: bl.room,
        teacher: bl.teacher
      },

      BlockTo: {
        blockId: blockTo.id,
        courseId: blockTo.courseId,
        day: blockTo.day,
        startHour: blockNumberToHour(blockTo.startBlock),
        duration: blockTo.endBlock - blockTo.startBlock,
        room: blockTo.room,
        teacher: blockTo.teacher
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
        if (exchangeMade === "") {
          window.alert("Žiadosť o výmenu bola evidovaná.");
          window.location.reload(false);
        } else {
          window.alert("Výmena bola vykonaná.");
          dispatch(loadMyTimetable(user.email));
        }
        dispatch(hideCourseTimetable(bl.id));
        dispatch(action);
        dispatch(loadExchangeRequests());
      })
      .catch((error) => {
        if (error.response) {
          window.alert("Pri vytváraní žiadosti nastala chyba.");
          // The request was made and the server responded with a status code
          // that falls out of the range of 2xx
        } else if (error.request) {
          window.alert("Nepodarilo sa nadviazať spojenie so serverom.");
          // The request was made but no response was received
          // `error.request` is an instance of XMLHttpRequest in the browser and an instance of
          // http.ClientRequest in node.js
        } else {
          // Something happened in setting up the request that triggered an Error
          console.log('Error', error.message);
          //This error shows undefined history after creating request
        }
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
      url: `/api/timetabledata/removeBlock/${userEmail}/${body.id}`
    })
    .then(() =>{
      dispatch({
        type: REMOVE_BLOCK_DONE
      });
      axios({
        method: 'get',
        url: '/api/timetabledata/getUserTimetable/' + userEmail
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
      url: `/api/timetabledata/addNewBlock`,
      data: body
    })
    .then(() =>{
      dispatch({
        type: ADD_BLOCK_DONE
      });
      axios({
        method: 'get',
        url: '/api/timetabledata/getUserTimetable/' + userEmail
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

export function addBlockAndHideOthersWithSameCourseId(body, userEmail) {
  return dispatch => {
    dispatch({
      type: ADD_BLOCK
    });
    axios({
      method: 'post',
      url: `/api/timetabledata/addNewBlock`,
      data: body
    })
    .then(() =>{
      dispatch({
        type: ADD_BLOCK_DONE
      });
      axios({
        method: 'get',
        url: '/api/timetabledata/getUserTimetable/' + userEmail
      })
        .then(res => {
          dispatch({
            type: HIDE_COURSE_TIMETABLE,
            payload: {
              timetable: res.data.blocks
            }
          });

          axios({
            method: 'get',
            url: '/api/timetabledata/getUserTimetable/' + userEmail
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
      url: `/api/timetabledata/editBlock`,
      data: body
    })
    .then(() =>{
      dispatch({
        type: EDIT_BLOCK_DONE
      });
      axios({
        method: 'get',
        url: '/api/timetabledata/getUserTimetable/' + userEmail
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

export function loadMyTimetableCalendar(user, history) {
  if (user.personalNumber == null) {
    history.push(PERSONALNUMBER);
  }
  return axios({
    method: 'get',
    url: '/api/timetabledata/getUserTimetableCalendar/' + user.email
  })
    .then(res => {
      return res.data;
    })
    .catch(() => {
      return null;
    });
}
