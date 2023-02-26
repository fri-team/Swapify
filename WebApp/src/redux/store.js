import { configureStore } from "@reduxjs/toolkit";
import {
  useDispatch as useAppDispatch,
  useSelector as useAppSelector,
} from "react-redux";
import { persistStore, persistReducer } from "redux-persist";
import { rootPersistConfig, rootReducerSec } from "./rootReducer";

// ----------------------------------------------------------------------

const storeRedux = configureStore({
  reducer: persistReducer(rootPersistConfig, rootReducerSec),
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: false,
      immutableCheck: false,
    }),
});

const persistor = persistStore(storeRedux);

const { dispatch } = storeRedux;

const useSelector = useAppSelector;

const useDispatch = () => useAppDispatch();

export { storeRedux, persistor, dispatch, useSelector, useDispatch };
