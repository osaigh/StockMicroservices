import {useState,useContext,useEffect} from "react";
import "./home.scss"
import classNames from 'classnames';
import { useSelector } from 'react-redux';
import { useDispatch } from "react-redux";
import { onAppClicked } from "../../slices/sidebar-slice";
import useWindowSize from '../../hooks/windowHook';
import { UserAuthenticationContext } from "../../context"; 
import Login from "../../components/login/Login";
import { Navigate } from "react-router-dom";
import { UserAuthenticationService } from "../../services";
import { User } from "oidc-client";

export default function Home(){
    const status = useSelector(state => (state as any).sidebar.status);
    const [user, setUser] = useState<User|null>(null);
    const userAuthenticationService = useContext<UserAuthenticationService|null>(UserAuthenticationContext);  
    const dispatch = useDispatch();
    const windowSize = useWindowSize();

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

    let contentClassList = classNames("homeContainer","maincontent");
    if(status == "hide"){
        contentClassList = classNames("homeContainer","maincontent","expand");
    }

    return (
      <>
      {user ?
        <Navigate to="/" />
        :
        <Login/>
        }
    </>
    );
}
