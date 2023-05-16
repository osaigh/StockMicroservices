import React, { useEffect, useState,useContext } from 'react';
import "./navbar.scss"
import FullScreenExitOutlinedIcon from '@mui/icons-material/FullscreenExitOutlined';
import NotificationsNoneOutlined from '@mui/icons-material/NotificationsNoneOutlined';
import ChatBubbleOutlinedOutlinedIcon from '@mui/icons-material/ChatBubbleOutlineOutlined'
import ListOutlinedIcon from '@mui/icons-material/ListOutlined';
import MenuIcon from '@mui/icons-material/Menu';
import { useDispatch } from 'react-redux';
import { toggleState } from '../../slices/sidebar-slice';
import { UserAuthenticationContext } from "../../context/user-manager-context";
import "bootstrap/dist/css/bootstrap.min.css";


const Navbar = () => {
    const dispatch = useDispatch();
    const [user, setUser] = useState(null);
    const { userAuthenticationService } = useContext(UserAuthenticationContext);  

    useEffect(() => {
        async function getUserAsync() {
          const user = await userAuthenticationService.getUser();
          return user;
        }
    
        getUserAsync().then((user) => {
          setUser(user);
        });
      }, []);

    const menuClicked = (e)=>{
        e.preventDefault();
        let newStatus = new Date().getTime();
        console.log(newStatus);
        dispatch(toggleState(newStatus));
    }
    
    const signOut = function () {
        userAuthenticationService.signOut();
    };
    
    const signIn = function () {
        userAuthenticationService.signIn();
    };

    let headerBar = "";
    if(user){
        headerBar = (
            <div className="items">
                    <div className="items">
                        <img src="user2-160x160.jpg"
                        alt=""
                        className='avatar'
                        />
                    </div>

                    <div className="items" >
                        <button  onClick={() => {signOut();}} className='btn btn-primary' > Sign Out </button>
                    </div>
                    
                </div>
        );
    }else{
        headerBar = (
            <div className="items">
                   
                    <div className="items" >
                        <button onClick={() => {signIn();}} className='btn btn-primary' style={{marginTop:0}}> Sign In </button>
                    </div>
                    
                </div>
        );
    }

    return (
        <div className='navbar'>
            <div className='wrapper' >
                <div className="menu" onClick={menuClicked}>
                    <MenuIcon className='icon'/>
                </div>
                {headerBar}
            </div>
        </div>
    );
};

export default Navbar;