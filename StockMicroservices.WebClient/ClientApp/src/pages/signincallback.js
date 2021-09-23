import React, { useState, useEffect, useContext } from "react";
import { Redirect, useHistory } from "react-router-dom";
import { UserAuthenticationContext } from "../context/user-manager-context";

export default function SignInCallback() {
  const [isRedirecting, setIsRedirecting] = useState(false);
  const { userAuthenticationService } = useContext(UserAuthenticationContext);
  const history = useHistory();
  console.log("SignInCallback called");
  useEffect(() => {
    async function getUserAsync() {
      const user = await userAuthenticationService.getUser();
    }

    console.log("SignInCallback UseEffect");
    const handler = () => {
      console.log("In signincallback, ");
      // getUserAsync().then((user) => {
      //   if (user) {
      //     console.log(history);
      //     setIsRedirecting(true);
      //   } else {
      //     setIsRedirecting(false);
      //   }
      // });
      //setRedirectAction(<Redirect to="/"></Redirect>);
      //history.push("");

      //history.push("/");
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
  }, [history]);
  console.log("In Signincallback: isRedirecting is " + isRedirecting);
  return isRedirecting ? (
    <Redirect to={{ pathname: "/" }} />
  ) : (
    <div>Redirecting</div>
  ); //<div>Redirecting</div>;
}
