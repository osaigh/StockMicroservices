import React, { useState, useEffect, useContext } from "react";
import { Navigate , useNavigate } from "react-router-dom";
import { UserAuthenticationContext } from "../../context/user-manager-context";

export function SignOutCallback() {
  const { userAuthenticationService } = useContext(UserAuthenticationContext);
  const navigate = useNavigate();
  console.log("SignOutCallback called");
  useEffect(() => {
    console.log("SignOutCallback UseEffect");
    const handler = () => {
      console.log("SignOutCallback handler");
      navigate.push("/");
    };

    async function signoutasync() {
      await userAuthenticationService
        .signoutRedirectCallback()
        .then(handler)
        .catch(function (e) {
          console.error(e);
        });
    }

    signoutasync();
  }, [navigate]);
  return <div>Redirecting</div>;
}
