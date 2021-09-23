import React from "react";
import {
  NavLink,
  UnOrderedList,
  ListItem,
  AsideContainer,
} from "./styles/page-sidebar";

export default function PageSideBar({ children, ...restProps }) {
  return <AsideContainer {...restProps}>{children}</AsideContainer>;
}

PageSideBar.NavLink = function PageSideBarNavLink({ children, ...restProps }) {
    return <NavLink className="nav-link" {...restProps}>{children}</NavLink>;
};

PageSideBar.UnOrderedList = function PageSideBarUnOrderedList({
  children,
  ...restProps
}) {
    return <UnOrderedList className="navbar-nav" {...restProps}>{children}</UnOrderedList>;
};

PageSideBar.ListItem = function PageSideBarListItem({
  children,
  ...restProps
}) {
    return <ListItem className="nav-item" {...restProps}>{children}</ListItem>;
};
