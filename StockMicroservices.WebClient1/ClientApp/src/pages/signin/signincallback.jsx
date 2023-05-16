import React, { useState, useEffect, useContext } from "react";
import { Navigate , useNavigate } from "react-router-dom";
import { UserAuthenticationContext } from "../../context/user-manager-context";

export function SignInCallback() {
  const [isRedirecting, setIsRedirecting] = useState(false);
  const { userAuthenticationService } = useContext(UserAuthenticationContext);
  const navigate = useNavigate();
  console.log("SignInCallback called");
  useEffect(() => {
    async function getUserAsync() {
      const user = await userAuthenticationService.getUser();
    }

    console.log("SignInCallback UseEffect");
    const handler = () => {
      console.log("In signincallback, ");

      setIsRedirecting(true);
    };

    async function signinasync() {
      await userAuthenticationService
        .signinRedirectCallback()
        .then(handler)
        .catch(function (e) {
          console.error(e);
        });
    }

    signinasync();
  }, [navigate]);
  console.log("In Signincallback: isRedirecting is " + isRedirecting);
  return isRedirecting ? (
    <Navigate  to="/" />
  ) : (
    <div>Redirecting</div>
  ); 
}
