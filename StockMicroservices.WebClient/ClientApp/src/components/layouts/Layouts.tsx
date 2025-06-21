import Sidebar from '../../components/sidebar/Sidebar';
import Navbar from '../../components/navbar/Navbar';
import { useSelector } from 'react-redux';
import Box from '@mui/material/Box';

export interface LayoutProps{
    children: any
}
function Layout(props:LayoutProps) {
    const status = useSelector(state => (state as any).sidebar.status);
    
    const containerStyle = {
        width:"100%", height:"100%"
    };

    let contentContainerStyle;

    if(status === 'hide'){
        contentContainerStyle ={
            width: 'calc(100% - 60px)',
            height: '200px',
            position: 'relative',
            top: '0',
            left: '60px',
            transition: '0.7s ease',
            '@media(max-width: 767px)' : {
                width: '100%',
                left: '0',
              }
        };
    }else{
        contentContainerStyle ={
            width: 'calc(100% - 280px)',
            height: '200px',
            position: 'relative',
            top: '0',
            left: '280px',
            transition: '0.7s ease',
            '@media(max-width: 767px)' : {
                width: '100%',
                left: '0',
              }
        };
    }

    

  return (
    <Box sx={containerStyle}>
        <Sidebar/>
        <Box sx={contentContainerStyle}>
            <Navbar/>
            <Box sx={{margin:3}}>

               {props.children}
            </Box>
        </Box>
    </Box>
  )
}

export default Layout