import React, { useState, useEffect, useContext } from "react";
import { Redirect, useHistory } from "react-router-dom";
import { UserAuthenticationContext } from "../context/user-manager-context";

export default function SignOutCallback() {
  const { userAuthenticationService } = useContext(UserAuthenticationContext);
  const history = useHistory();
  console.log("SignOutCallback called");
  useEffect(() => {
    console.log("SignOutCallback UseEffect");
    const handler = () => {
      console.log("SignOutCallback handler");
      history.push("/");
    };

    async function signoutasync() {
      //userManagerService.userManager.clearStaleState();
      //userManagerService.userManager.removeUser();
      await userAuthenticationService
        .signoutRedirectCallback()
        .then(handler)
        .catch(function (e) {
          console.error(e);
        });
    }

    signoutasync();
  }, [history]);
  return <div>Redirecting</div>;
}
