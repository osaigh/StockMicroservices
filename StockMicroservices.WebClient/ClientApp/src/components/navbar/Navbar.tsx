import { useEffect, useState,useContext } from 'react';
import "./navbar.scss"
import MenuIcon from '@mui/icons-material/Menu';
import { useDispatch } from 'react-redux';
import { toggleState } from '../../slices/sidebar-slice';
import { UserAuthenticationContext } from '../../context';
import { UserAuthenticationService } from '../../services';
import "bootstrap/dist/css/bootstrap.min.css";
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import Avatar from '@mui/material/Avatar';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import { User } from 'oidc-client';


const Navbar = () => {
    const [anchorElUser, setAnchorElUser] = useState(null);
    const dispatch = useDispatch();
    const [user, setUser] = useState<User|null>(null);
    const userAuthenticationService = useContext<UserAuthenticationService|null>(UserAuthenticationContext);  

    useEffect(() => {
      async function getUserAsync() {
          const user = await userAuthenticationService?.getUser();
          return user;
        }
    
        getUserAsync().then((user) => {
          if(user){
            setUser(user);
          }
        });
      }, []);

    const menuClicked = (e:any)=>{
        e.preventDefault();
        let newStatus = new Date().getTime();
        console.log(newStatus);
        dispatch(toggleState(newStatus));
    }

    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
      };

    const handleOpenUserMenu = (event:any) => {
        setAnchorElUser(event.currentTarget);
      };
    
    const signOut = function () {
        userAuthenticationService?.signOut();
    };
    
    let headerBar;
    if(user){
        headerBar = (
            <Box sx={{ flexGrow: 0 }}>
            <Tooltip title="Open settings">
              <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                <Avatar alt="Osa " src="/user2-160x160.jpg" />
              </IconButton>
            </Tooltip>
            <Menu
              sx={{ mt: '45px' }}
              id="menu-appbar"
              anchorEl={anchorElUser}
              anchorOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              keepMounted
              transformOrigin={{
                vertical: 'top',
                horizontal: 'right',
              }}
              open={Boolean(anchorElUser)}
              onClose={handleCloseUserMenu}
            >
              <MenuItem onClick={() => {signOut();}}>
                  <Typography textAlign="center">Sign Out</Typography>
                </MenuItem>
            </Menu>
          </Box>
        );
    }else{
        headerBar = (
            <Button color="inherit">Login</Button>
        );
    }

    return (
        <Box sx={{ flexGrow: 1 }}>
            <AppBar position="static">
            <Toolbar>
                <IconButton
                size="large"
                edge="start"
                color="inherit"
                aria-label="menu"
                sx={{ mr: 2 }}
                onClick={menuClicked}
                >
                <MenuIcon />
                </IconButton>
                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                
                </Typography>
                {headerBar}
            </Toolbar>
            </AppBar>
        </Box>
    );
};

export default Navbar;