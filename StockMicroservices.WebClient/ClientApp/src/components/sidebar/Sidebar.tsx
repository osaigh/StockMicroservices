import "./sidebar.scss"
import { useSelector } from 'react-redux';
import classNames from 'classnames';
import { Link } from "react-router-dom";
import useWindowSize, { WindowSize } from '../../hooks/windowHook';
import Box from '@mui/material/Box';
import ListSubheader from '@mui/material/ListSubheader';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import SellIcon from '@mui/icons-material/Sell';
import AccountBalanceIcon from '@mui/icons-material/AccountBalance';

const Sidebar = () => {
    const status = useSelector(state => (state as any).sidebar.status);
    const windowSize :WindowSize = useWindowSize();

    
    let sidebarClassList = classNames('sidebar');
    let headingClassList = classNames('title');
    let spanClassList = classNames('');
    let brandClassList = classNames("brand");

    const brandLinkStyle: {
        height: string,
        display: string,
        alignItems: string,
        fontWeight: string,
        justifyContent:string,
        overflow: string,
        textDecoration: string,
        color: string,
        fontSize: string,
        borderBottom: string,
        visibility: any
    } = {
        height: '64px',
        display: 'flex',
        alignItems: 'center',
        fontWeight: '300',
        justifyContent: 'center',
        overflow: 'hidden',
        textDecoration: 'none',
        color: 'white',
        fontSize: '20px',
        borderBottom: '1px solid rgb(200,200,200)',
        visibility: "visible"
    };
    const sideBarContainerStyle = {
        position: 'fixed',
        top:'0',
        left: '0',
        width: '280px',
        zIndex: '1000',
        borderRight: '0.5px solid rgb(230,227,227)',
        minHeight: '100vh',
        backgroundColor: '#006AC3',
        transition: '.7s ease',
        boxShadow: '2px 2px 10px 1px rgb(185, 183, 183)',
        
    };
    const subHeadingStyle = {
        backgroundColor:'transparent',
         color:'white',
         visibility: 'visible'
    };

    const iconStyle = {
        color: 'white',
        visibility: 'visible',
    };
    const listItemStyle = {
        '&:hover':{
            backgroundColor: 'rgba(255,255,255,0.2)',
        }
        
    }

    const listItemTextStyle = {
        color: 'white',
        marginLeft:"0", 
        paddingTop:'1px',
        display:"visible"
    }

    if(windowSize.width  && windowSize.width < 767){
        if(status.length > 0 ){
            
            sidebarClassList = classNames('sidebar',"show");
            headingClassList = classNames('title');
            brandClassList = classNames("brand");
            subHeadingStyle.visibility= 'visible';
            sideBarContainerStyle.width = '280px';
            brandLinkStyle.visibility = 'visible';
        }else{
            subHeadingStyle.visibility= 'collapse';
            iconStyle.visibility = 'collapse';
            brandLinkStyle.visibility = 'collapse';
            sideBarContainerStyle.width = '0px';
        }
    }else{
        if(status.length > 0 ){
            subHeadingStyle.visibility= 'collapse';
            listItemTextStyle.display = 'none';
            if(status === 'hide'){
                sideBarContainerStyle.width = '60px';
            }else{
                sideBarContainerStyle.width = '280px';
            }
            sidebarClassList = classNames('sidebar',status);
            headingClassList = classNames('title','hideheading');
            spanClassList = classNames('hidespan');
            brandClassList = classNames("brand",'hideheading');
            brandLinkStyle.visibility = 'collapse';
        }
    }
   
    return (
        <Box sx={sideBarContainerStyle}>
            <Box  ><Link  to="/" style={brandLinkStyle}>Stock Trader</Link></Box>
           
            <Box >
                <List
                    component="nav"
                    aria-labelledby="nested-list-subheader"
                    subheader={
                      <ListSubheader sx={subHeadingStyle} component="div" id="nested-list-subheader">
                        Main
                      </ListSubheader>
                    }
                >
                    <Link to="/" style={{ textDecoration: "none" }}>
                        <ListItem sx={listItemStyle} >
                            <ListItemIcon >
                                <AccountBalanceIcon sx={iconStyle}/>
                            </ListItemIcon >
                            <ListItemText sx={listItemTextStyle}  primary="Stocks" />
                        </ListItem>
                    </Link>
                    <Link to="/orders" style={{ textDecoration: "none" }}>
                        <ListItem sx={listItemStyle}>
                            <ListItemIcon >
                                <SellIcon sx={iconStyle}/>
                            </ListItemIcon >
                            <ListItemText sx={listItemTextStyle} primary="Orders" />
                        </ListItem>
                    </Link>
                    
                </List>
            </Box>
            
        </Box>
    );
};

export default Sidebar;