import { useState, useContext, useEffect } from "react";
import "./home.scss";
import classNames from "classnames";
import { useSelector } from "react-redux";
import Sidebar from "../../components/sidebar/Sidebar";
import Navbar from "../../components/navbar/Navbar";
import { UserAuthenticationContext } from "../../context/user-manager-context";
import { StockContainer } from "../../components/stockcontainer/Stockcontainer";

export default function Home() {
  const status = useSelector((state) => state.sidebar.status);
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

  let contentClassList = classNames("homeContainer", "maincontent");
  if (status === "hide") {
    contentClassList = classNames("homeContainer", "maincontent", "expand");
  }

  return (
    <div className="home">
      <Sidebar />
      <div className={contentClassList}>
        <Navbar />
        <div className="content">
          {user ? (
            <StockContainer />
          ) : (
            <div
              style={{
                marginLeft: "auto",
                marginRight: "auto",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                height: "300px",
              }}
            >
              <h6>Please sign in to view stocks.</h6>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
