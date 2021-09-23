import React from "react";
import classNames from "classnames";
import { PageHeader } from "../components";
import "bootstrap/dist/css/bootstrap.min.css";
import Button from "react-bootstrap/Button";

export function PageHeaderContainer({
  signIn,
  signOut,
  userGreeting,
  children,
  isUserSignedIn,
}) {
  var navClassNames = classNames(
    "navbar",
    "navbar-expand-lg",
    "navbar-light",
    "bg-light"
  );

  var brandLinkClassNames = classNames("navbar-brand");
  var togglerClassNames = classNames("navbar-toggler");
  var togglerIconClassNames = classNames("navbar-toggler-icon");
  var collapseDivClassNames = classNames("collapse", "navbar-collapse");
  var navItemClassNames = classNames("nav-item");
  var navLinkClassNames = classNames("nav-link");
  var mlAutoClassNames = classNames("ml-auto");
  var mrAutoClassNames = classNames("mr-auto");
  var navBarNavClassNames = classNames("navbar-nav");
  return isUserSignedIn ? (
    <PageHeader>
      <PageHeader.Nav className={navClassNames}>
        <PageHeader.NavLink to="/" className={brandLinkClassNames}>
          Stock
        </PageHeader.NavLink>
        <Button
          className={togglerClassNames}
          type="button"
          data-toggler="collapse"
          data-target="#navbarSupportedContent"
        >
          <PageHeader.NavSpan
            className={togglerIconClassNames}
          ></PageHeader.NavSpan>
        </Button>
        <PageHeader.DivContainer
          className={collapseDivClassNames}
          id="navbarSupportedContent"
        >
          <PageHeader.DivContainer className="mr-auto"></PageHeader.DivContainer>
          <PageHeader.DivContainer className={mlAutoClassNames}>
            <PageHeader.UnOrderedList className={navBarNavClassNames}>
              <PageHeader.ListItem>{userGreeting}</PageHeader.ListItem>
              <PageHeader.ListItem className={navItemClassNames}>
                              <Button style={{ color:"white"}
}
                  className={navLinkClassNames}
                  onClick={() => {
                    signOut();
                  }}
                >
                  Sign Out
                </Button>
              </PageHeader.ListItem>
            </PageHeader.UnOrderedList>
          </PageHeader.DivContainer>
        </PageHeader.DivContainer>
      </PageHeader.Nav>
    </PageHeader>
  ) : (
    <PageHeader>
      <PageHeader.Nav className={navClassNames}>
        <PageHeader.NavLink to="/" className={brandLinkClassNames}>
          Stock
        </PageHeader.NavLink>
        <Button
          className={togglerClassNames}
          type="button"
          data-toggler="collapse"
          data-target="#navbarSupportedContent"
        >
          <PageHeader.NavSpan
            className={togglerIconClassNames}
          ></PageHeader.NavSpan>
        </Button>
        <PageHeader.DivContainer
          className={collapseDivClassNames}
          id="navbarSupportedContent"
        >
          <PageHeader.DivContainer className="mr-auto"></PageHeader.DivContainer>
          <PageHeader.DivContainer className={mlAutoClassNames}>
            <PageHeader.UnOrderedList className={navBarNavClassNames}>
              <PageHeader.ListItem className={navItemClassNames}>
                <Button style={{ color:"white"}}
                  className={navLinkClassNames}
                  onClick={() => {
                    signIn();
                  }}
                >
                  Sign In
                </Button>
              </PageHeader.ListItem>
            </PageHeader.UnOrderedList>
          </PageHeader.DivContainer>
        </PageHeader.DivContainer>
      </PageHeader.Nav>
    </PageHeader>
  );
}
