import "./datatable.scss";
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import Box from '@mui/material/Box';
import { TableItem } from "../../models";

export interface DatatableProps{
    width?:string|number,
    height?:string | number,
    columns: GridColDef[],
    rowData:any[],
    children?: any
}
const Datatable = (props:DatatableProps) => {

  let width :string|number = 0;
  let height : string|number = 0;

  if(props.width && props.height){
    width = props.width;
    height = props.height;
  }else if(props.width){
    width = props.width;
    height = "100%";
  }else if(props.height){
    height = props.height;
    width = "100%";
  }else {
    width = "100%";
    height = "400px";
  }
  
  return (
    <Box sx={{width:width, height:height}}>
      <DataGrid
        
        rows={props.rowData}
        columns={props.columns}
      />
    </Box>
  );
};

export default Datatable;