import { useEffect, useState, useContext } from "react";
import "./navbar.scss";
import MenuIcon from "@mui/icons-material/Menu";
import { useDispatch } from "react-redux";
import { toggleState } from "../../slices/sidebar-slice";
import { UserAuthenticationContext } from "../../context/user-manager-context";
import "bootstrap/dist/css/bootstrap.min.css";

const Navbar = () => {
  const dispatch = useDispatch();
  const [user, setUser] = useState(null);
  const { userAuthenticationService } = useContext(UserAuthenticationContext);

  useEffect(() => {
    let cancelled = false;

    userAuthenticationService.getUser().then((currentUser) => {
      if (!cancelled) {
        setUser(currentUser);
      }
    });

    return () => {
      cancelled = true;
    };
  }, [userAuthenticationService]);

  const menuClicked = (event) => {
    event.preventDefault();
    dispatch(toggleState(Date.now()));
  };

  const displayName = user?.profile?.name || user?.profile?.sub || "Account";

  return (
    <div className="navbar">
      <div className="wrapper">
        <div className="menu" onClick={menuClicked} role="button" tabIndex={0}>
          <MenuIcon className="icon" />
        </div>
        <div className="items">
          {user ? (
            <>
              <span className="nav-user">{displayName}</span>
              <button
                type="button"
                onClick={() => userAuthenticationService.signOut()}
                className="btn btn-primary"
              >
                Sign Out
              </button>
            </>
          ) : (
            <button
              type="button"
              onClick={() => userAuthenticationService.signIn()}
              className="btn btn-primary"
              style={{ marginTop: 0 }}
            >
              Sign In
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default Navbar;
