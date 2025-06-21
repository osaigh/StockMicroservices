import { useEffect, useContext } from "react";
import {  NavigateFunction, useNavigate } from "react-router-dom";
import { UserAuthenticationContext } from "../../context";
import { UserAuthenticationService } from "../../services";

export function SignOutCallback() {
  const userAuthenticationService = useContext<UserAuthenticationService | null>(UserAuthenticationContext);
  const navigate: NavigateFunction = useNavigate();
  console.log("SignOutCallback called");
  useEffect(() => {
    console.log("SignOutCallback UseEffect");
    const handler = () => {
      console.log("SignOutCallback handler");
      navigate("/");
    };

    async function signoutasync() {
      await userAuthenticationService?.signoutRedirectCallback()
        .then(handler)
        .catch(function (e) {
          console.error(e);
        });
    }

    signoutasync();
  }, [navigate]);
  return <div>Redirecting</div>;
}
