import { createSlice, PayloadAction } from "@reduxjs/toolkit";

export const appSlice = createSlice({
    name:"app",
    initialState: {
        screen:""
    },
    reducers:{
        toggleScreen :(state, action)=>{
            const newState = {...state};
            newState.screen = action.payload;

            return newState;
        }
    }
});

export const {toggleScreen} = appSlice.actions;

export default appSlice.reducer;