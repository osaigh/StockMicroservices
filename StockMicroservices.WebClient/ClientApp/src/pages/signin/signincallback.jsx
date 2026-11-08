import { useContext, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { UserAuthenticationContext } from "../../context/user-manager-context";

export function SignInCallback() {
  const { userAuthenticationService } = useContext(UserAuthenticationContext);
  const navigate = useNavigate();

  useEffect(() => {
    let cancelled = false;

    userAuthenticationService
      .signinRedirectCallback()
      .then(() => {
        if (!cancelled) {
          navigate("/", { replace: true });
        }
      })
      .catch((error) => {
        console.error("Sign-in callback failed", error);
      });

    return () => {
      cancelled = true;
    };
  }, [navigate, userAuthenticationService]);

  return <div>Signing you in...</div>;
}
