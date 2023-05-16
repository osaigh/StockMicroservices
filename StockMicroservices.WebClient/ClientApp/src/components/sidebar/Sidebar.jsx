import React from 'react';
import "./sidebar.scss"
import HomeIcon from "@mui/icons-material/Home";
import { useSelector } from 'react-redux';
import classNames from 'classnames';
import { Link } from "react-router-dom";
import useWindowSize from '../../hooks/windowHook';

const Sidebar = () => {
    const status = useSelector(state => state.sidebar.status);
    const windowSize  = useWindowSize();

    let sidebarClassList = classNames('sidebar');
    let headingClassList = classNames('title');
    let spanClassList = classNames('');
    let brandClassList = classNames("brand");
    console.log("SideBar");
    if(windowSize.width < 767){
        if(status.length > 0 ){
            sidebarClassList = classNames('sidebar',"show");
            headingClassList = classNames('title');
            brandClassList = classNames("brand");
        }
    }else{
        if(status.length > 0 ){
            sidebarClassList = classNames('sidebar',status);
            headingClassList = classNames('title','hideheading');
            spanClassList = classNames('hidespan');
            brandClassList = classNames("brand",'hideheading');
        }
    }
   
    return (
        <div className={sidebarClassList}>
            <div className='top'><Link className="homelink" to="/" style={{ textDecoration: "none" }}><span className={brandClassList}>Stock Apex</span> </Link></div>
            <hr/>
            <div className='center'>
                <ul>
                    <p className={headingClassList}>Main</p>
                    <Link to="/" style={{ textDecoration: "none" }}><li><HomeIcon className='icon'/><span className={spanClassList}>Home</span></li></Link>
                    
                </ul>
            </div>
            
        </div>
    );
};

export default Sidebar;