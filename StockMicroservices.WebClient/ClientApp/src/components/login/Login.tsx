import {useContext, useEffect,useState} from 'react';
import Stack from '@mui/material/Stack';
import {Typography } from '@mui/material';
import { useDispatch } from 'react-redux';
import { UserAuthenticationContext } from "../../context";
import { UserAuthenticationService } from '../../services';
import { User } from 'oidc-client';

function Login() {
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
    
      const signIn = function () {
        userAuthenticationService?.signIn();
    };

  return (
    <Stack
        direction="row"
        justifyContent="center"
        alignItems="center"
        sx={{
            width:"100vw",
            height:"100vh"
        }}
    >
        <Stack
        direction="column"
        justifyContent="center"
        alignItems="center"
        spacing={6}
            sx={{
                marginLeft:"auto",
                marginRight:"auto",
                width: 400,
                height: 300,
                borderRadius:"10px",
                border:"solid 0.5px rgba(100,100,100,0.4)",
                boxShadow:"2px 2px 10px 1px rgb(185, 183, 183)"
            }}
        >
            <Typography variant="h4" component="div"
                sx={{
                    marginTop:1
                }}
            >
                    Stock Trader
            </Typography>
            <Stack 
                direction="column"
                alignItems="center"
                spacing={2}
                sx={{width:300}}>
               <button onClick={() => {signIn();}} className='btn btn-primary' style={{marginTop:0}}> Sign In </button>
    
            </Stack>
            
        </Stack>
    </Stack>
  )
}

export default Login