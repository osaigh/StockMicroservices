import { createSlice } from "@reduxjs/toolkit";

export const appSlice = createSlice({
  name: "app",
  initialState: {
    screen: "",
  },
  reducers: {
    toggleScreen: (state, action) => {
      state.screen = action.payload;
    },
  },
});

export const { toggleScreen } = appSlice.actions;

export default appSlice.reducer;
