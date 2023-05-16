import { createSlice, PayloadAction } from "@reduxjs/toolkit";

export const sidebarSlice = createSlice({
    name:"sidebar",
    initialState: {
        status:""
    },
    reducers:{
        toggleState :(state, action)=>{
            const newState = {...state};
            if(newState.status == "hide"){
                newState.status = "";
            }else{
                newState.status = "hide";
            }

            return newState;
        },
        onAppClicked :(state, action)=>{
            const newState = {...state};
            newState.status = "";

            return newState;
        }
    }
});

export const {toggleState,onAppClicked} = sidebarSlice.actions;

export default sidebarSlice.reducer;