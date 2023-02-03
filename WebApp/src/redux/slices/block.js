import { createSlice } from "@reduxjs/toolkit";
// utils
import axios from "axios";
//import { userInfo } from "os";
//
import { dispatch } from "../store";

// ----------------------------------------------------------------------

const initialState = {
  isLoading: false,
  error: null,
  eventName: "",
  eventPlace: "",
  startBlock: null,
  endBlock: null,
  blockColor: "",
  type: 4,
};

const slice = createSlice({
  name: "block",
  initialState,
  reducers: {
    // START LOADING
    startLoading(state) {
      state.isLoading = true;
    },

    // HAS ERROR
    hasError(state, action) {
      state.isLoading = false;
      state.error = action.payload;
      console.log(state.error);

      window.alert("Nepodarilo sa pridat blok, skúste to neskôr prosím.");
    },

    // CREATE BLOCK
    createBlockSuccess(state, action) {
      const newBlock = action.payload;
      state.isLoading = false;
      console.log(newBlock);
    },
  },
});

// Reducer
export default slice.reducer;

// ----------------------------------------------------------------------

export function createBlock(newBlock) {
  return async () => {
    dispatch(slice.actions.startLoading());
    try {
      console.log(newBlock);
      const response = await axios.post("/api/student/addNewBlock", newBlock);
      console.log(response);
      dispatch(slice.actions.createBlockSuccess(response.data));
      window.location.reload(false);
    } catch (error) {
      dispatch(slice.actions.hasError(error));
    }
  };
}
