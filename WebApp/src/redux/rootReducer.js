import { combineReducers } from "redux";
import storage from "redux-persist/lib/storage";

// slices
import blockReducer from "./slices/block";

// ----------------------------------------------------------------------

const rootPersistConfig = {
  key: "root",
  storage,
  keyPrefix: "redux-",
  whitelist: [],
};

const rootReducerSec = combineReducers({
  block: blockReducer,
});

export { rootPersistConfig, rootReducerSec };
