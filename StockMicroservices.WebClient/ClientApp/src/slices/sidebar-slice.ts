import { createSlice } from "@reduxjs/toolkit";

export interface SidebarState{
  status:string
}

export const sidebarSlice = createSlice({
  name: "sidebar",
  initialState: {
    status: "",
  },
  reducers: {
    toggleState: (state:SidebarState, action) => {
      const newState = { ...state };
      if (newState.status == "hide") {
        newState.status = "";
      } else {
        newState.status = "hide";
      }

      return newState;
    },
    onAppClicked: (state:SidebarState, action) => {
      const newState = { ...state };
      newState.status = "";

      return newState;
    },
  },
});

export const { toggleState, onAppClicked } = sidebarSlice.actions;

export default sidebarSlice.reducer;
