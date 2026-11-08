import { useEffect, useContext } from "react";
import { useNavigate } from "react-router-dom";
import { UserAuthenticationContext } from "../../context/user-manager-context";

export function SignOutCallback() {
  const { userAuthenticationService } = useContext(UserAuthenticationContext);
  const navigate = useNavigate();

  useEffect(() => {
    let cancelled = false;

    userAuthenticationService
      .signoutRedirectCallback()
      .then(() => {
        if (!cancelled) {
          navigate("/", { replace: true });
        }
      })
      .catch((error) => {
        console.error("Sign-out callback failed", error);
      });

    return () => {
      cancelled = true;
    };
  }, [navigate, userAuthenticationService]);

  return <div>Signing you out...</div>;
}
