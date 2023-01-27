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
  SHOW_EXCHANGE_MODE_TIMETABLE,
  CONFIRM_EXCHANGE_REQUEST,
  CANCEL_EXCHANGE_MODE,
  ADD_BLOCK,
  ADD_BLOCK_DONE,
  ADD_BLOCK_FAIL,
  CHOOSE_EXCHANGE_FROM_BLOCK
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
  addedBlocks: [],
  isExchangeMode: false,
  isAddBlockMode: false,
  isBlockRemoved: false,
  blockFromExchange: null,
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
        myCourseNames: _.sortBy(_.uniqBy(_.map(payload.timetable, function (item) {
          return { courseId: item.courseId, courseName: item.courseName, courseType : item.type};
        }), 'courseId'), 'courseName'),
        displayedTimetable: mergeTimetables(
          payload.timetable,
          _.pick(state.courseTimetables, state.displayedCourses),
          0
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
          _.set({}, payload.course.courseId, payload.course.timetable)
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
        isAddBlockMode: true,
        displayedCourses: _.concat(state.displayedCourses, payload.courseId),
        displayedTimetable: mergeTimerablesShow(
          state.myTimetable,
          _.pick(
            state.courseTimetables,
            _.concat(state.displayedCourses, payload.courseId)
          ),
          state.displayedCourses.length + 1
        )
      };
    case SHOW_EXCHANGE_MODE_TIMETABLE:
      return {
        ...state,
        displayedCourses: _.concat(state.displayedCourses, payload.courseId),
        displayedTimetable: mergeTimerablesShow(
          state.myTimetable,
          _.pick(
            state.courseTimetables,
            _.concat(state.displayedCourses, payload.courseId)
          ),
          state.displayedCourses.length + 1
        )
      };
    case CANCEL_EXCHANGE_MODE:
      return {
        ...state,
        isExchangeMode: false,
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(state.courseTimetables, state.displayedCourses),
          state.displayedCourses.length - 1
        ),
        blockFromExchange: null
      }
    case CONFIRM_EXCHANGE_REQUEST:
      return {
        ...state,
        isExchangeMode: false,
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(state.courseTimetables, state.displayedCourses),
          state.displayedCourses.length + 1
        ),
        blockFromExchange: null
      }
    case HIDE_COURSE_TIMETABLE:
      var displayedCourses;
      // if course id set to null hide all courses
      if (payload.courseId == null) {
        displayedCourses = [];
      } else {
        displayedCourses = _.without(state.displayedCourses, payload.courseId);
      }

      return {
        ...state,
        isAddBlockMode: false,
        displayedCourses: displayedCourses,
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(
            state.courseTimetables,
            displayedCourses
          ),
          state.displayedCourses.length - 1
        )
      };
    case REMOVE_BLOCK:
      return {
        ...state,
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(state.courseTimetables, state.displayedCourses),
          0
        ),
        isBlockRemoved: false
      };
    case REMOVE_BLOCK_DONE:
      return {
        ...state,
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(state.courseTimetables, state.displayedCourses),
          0
        ),
        isBlockRemoved: true
      };
    case REMOVE_BLOCK_FAIL:
      return {
        ...state,
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(state.courseTimetables, state.displayedCourses),
          0
        ),
        isBlockRemoved: false
      };
    case EDIT_BLOCK:
      return {
        ...state,
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(state.courseTimetables, state.displayedCourses),
          0
        ),
        isEdited: false
      };
    case EDIT_BLOCK_DONE:
      return {
        ...state,
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(state.courseTimetables, state.displayedCourses),
          0
        ),
        isEdited: true
      };
    case EDIT_BLOCK_FAIL:
      return {
        ...state,
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(state.courseTimetables, state.displayedCourses),
          0
        ),
        isEdited: false
      };
    case ADD_BLOCK:
      return {
        ...state,
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(state.courseTimetables, state.displayedCourses),
          0
        ),
        isAdded: false
      };
    case ADD_BLOCK_DONE:
      return {
        ...state,
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(state.courseTimetables, state.displayedCourses),
          0
        ),
        isAdded: true
      };
    case ADD_BLOCK_FAIL:
      return {
        ...state,
        displayedTimetable: mergeTimetables(
          state.myTimetable,
          _.pick(state.courseTimetables, state.displayedCourses),
          0
        ),
        isAdded: false
      };
    case CHOOSE_EXCHANGE_FROM_BLOCK:
      return {
        ...state,
        isExchangeMode: true,
        blockFromExchange: payload.course
      }
    default:
      return state;
  }
}

export function mergeTimetables(myTimetable, courseTimetables, numOfShowedCourses) {
  if (_.isNil(courseTimetables)) {
    return myTimetable;
  }
  const coursesWithoutMyLabs = _.map(courseTimetables, cTimetable =>
    _.differenceWith(cTimetable, myTimetable, _.isEqual)
  );
  const myFlaggedTimetable = _.map(myTimetable, b => {
    if (numOfShowedCourses == 0) {
      return _.merge({}, b, { isGrey: true, isMine: true })
    } else {
      return _.merge({}, b, { isGrey: false, isMine: true })
    }
  });
  return _.concat(myFlaggedTimetable, _.flatten(coursesWithoutMyLabs));
}

export function mergeTimerablesShow(myTimetable, courseTimetables, numOfShowedCourses) {
  if (_.isNil(courseTimetables)) {
    return myTimetable;
  }
  const coursesWithoutMyLabs = _.map(courseTimetables, cTimetable =>
    _.differenceWith(cTimetable, myTimetable, _.isEqual)
  );
  const myFlaggedTimetable =
    _.map(myTimetable, b => {
      if (numOfShowedCourses > 0) {
        return _.merge({}, b, { isGrey: false, isMine: true })
      } else {
        return _.merge({}, b, { isGrey: true, isMine: true })
      }
    });
  return _.concat(myFlaggedTimetable, _.flatten(coursesWithoutMyLabs));
}
