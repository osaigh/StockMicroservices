import React, {useState,useContext,useEffect} from "react";
import { UserAuthenticationService } from "../../services";
import LoginComponent from "../../components/login/Login";
import { Navigate } from "react-router-dom";
import { UserAuthenticationContext } from "../../context";
import { User } from "oidc-client";

export default function Login(){
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

    return (
      <>
      {user ?
        <Navigate to="/" />
        :
        <LoginComponent/>
        }
    </>
    );
}