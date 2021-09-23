import React, { useContext, useEffect, useState } from "react";
import PageTemplate from "./page-template";
import { UserAuthenticationContext } from "../context/user-manager-context";
import { PageSideBar, PageHeader } from "../components";
import classNames from "classnames";
import { StockContainer } from "../containers/stock-container";
import "@fortawesome/fontawesome-free/css/all.min.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "../assets/site.css";

export default function Home() {
  const [user, setUser] = useState(null);
  const { userAuthenticationService } = useContext(UserAuthenticationContext);

  const signOut = function () {
    userAuthenticationService.signOut();
  };
  const signIn = function () {
    userAuthenticationService.signIn();
  };
  useEffect(() => {
    async function getUserAsync() {
      const user = await userAuthenticationService.getUser();
      return user;
    }

    getUserAsync().then((user) => {
      setUser(user);
    });
  }, []);

  console.log("in Home");
    const sideBarHome = (
        <PageSideBar.UnOrderedList >
            <PageSideBar.ListItem>
                <PageSideBar.NavLink to="/">Home</PageSideBar.NavLink>
            </PageSideBar.ListItem>
        </PageSideBar.UnOrderedList>
        );

  var userGreetingClassNames = classNames("mt-2", "mr-1");
  var userGreeting = {};
  var content = {};
  if (user) {
    var message = "Hello " + user["profile"]["name"] + "!";
    userGreeting = (
      <PageHeader.UserDiv>
        <PageHeader.UserGreeting className={userGreetingClassNames}>
          {message}
        </PageHeader.UserGreeting>
      </PageHeader.UserDiv>
    );

    content = (
      <PageTemplate
        userGreeting={userGreeting}
        sidebarlinks={sideBarHome}
        signIn={signIn}
        signOut={signOut}
        isUserSignedIn={true}
      >
        <StockContainer></StockContainer>
      </PageTemplate>
    );
  } else {
    content = (
      <PageTemplate
        userGreeting={userGreeting}
        sidebarlinks={sideBarHome}
        signIn={signIn}
        signOut={signOut}
        isUserSignedIn={false}
      ></PageTemplate>
    );
  }

  return <>{content}</>;
}
