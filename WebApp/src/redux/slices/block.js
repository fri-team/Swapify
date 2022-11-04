import { createSlice } from "@reduxjs/toolkit";
// utils
import axios from "axios";
//
//import { dispatch } from "../store";

// ----------------------------------------------------------------------

const initialState = {
  isLoading: false,
  error: null,
  eventName: "",
  eventPlace: "",
  startBlock: null,
  endBlock: null,
  blockColor: "",
};

const slice = createSlice({
  name: "product",
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
    },

    // GET PRODUCTS
    getProductsSuccess(state, action) {
      state.isLoading = false;
      state.products = action.payload;
    },
  },
});

// Reducer
export default slice.reducer;

// ----------------------------------------------------------------------

export function getCustomBlock() {
  return async (dispatch) => {
    dispatch(slice.actions.startLoading());
    try {
      const response = await axios.get("/api/products");
      dispatch(slice.actions.getProductsSuccess(response.data.products));
    } catch (error) {
      dispatch(slice.actions.hasError(error));
    }
  };
}

// ----------------------------------------------------------------------

/*fetchCustomBlock = (eventName) => {
  this.setState({ eventPlace: eventName });

  const startBlock = parseInt(this.state.startBlock.split(":")[0]);
  axios
    .get(`/api/timetable/getCustomBlock/${startBlock}/${this.state.day}`)
    .then(({ data }) => {
      this.setState({ eventPlace: data.eventPlace });
      this.setState({ length: data.endBlock - data.startBlock });
    })
    .catch(function (error) {
      if (error.response.status == "404") {
        alert("Upozornenie: Problém pri načítaní vlastnej udalosti");
      }
    });
};
*/
