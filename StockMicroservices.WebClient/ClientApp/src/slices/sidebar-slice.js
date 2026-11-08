import { createSlice } from "@reduxjs/toolkit";

export const sidebarSlice = createSlice({
  name: "sidebar",
  initialState: {
    status: "",
  },
  reducers: {
    toggleState: (state) => {
      state.status = state.status === "hide" ? "" : "hide";
    },
    onAppClicked: (state) => {
      state.status = "";
    },
  },
});

export const { toggleState, onAppClicked } = sidebarSlice.actions;

export default sidebarSlice.reducer;
