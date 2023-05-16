import { configureStore ,Store} from "@reduxjs/toolkit";
import sidebarReducer from './slices/sidebar-slice';
import appReducer from './slices/app-slice';

export const store = configureStore({
    reducer:{
        sidebar:sidebarReducer,
        app: appReducer
    }
});