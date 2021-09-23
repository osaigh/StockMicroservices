import React from "react";
import classNames from "classnames";
import {
  UserDiv,
  UserGreeting,
  NavLink,
  Nav,
  UnOrderedList,
  ListItem,
  DivContainer,
  NavSpan,
} from "./styles/page-header";

export default function PageHeader({ children, ...restProps }) {
  return <header>{children}</header>;
}

PageHeader.Nav = function PageHeaderNav({ children, ...restProps }) {
  return <Nav {...restProps}>{children}</Nav>;
};

PageHeader.NavLink = function PageHeaderNavLink({ children, ...restProps }) {
  return <NavLink {...restProps}>{children}</NavLink>;
};

PageHeader.DivContainer = function PageHeaderDivContainer({
  children,
  ...restProps
}) {
  return <DivContainer {...restProps}>{children}</DivContainer>;
};

PageHeader.NavSpan = function PageHeaderNavSpan({ children, ...restProps }) {
  return <NavSpan {...restProps}>{children}</NavSpan>;
};

PageHeader.UnOrderedList = function PageHeaderUnOrderedList({
  children,
  ...restProps
}) {
  return <UnOrderedList {...restProps}>{children}</UnOrderedList>;
};

PageHeader.ListItem = function PageHeaderListItem({ children, ...restProps }) {
  return <ListItem {...restProps}>{children}</ListItem>;
};

PageHeader.UserDiv = function PageHeaderUserDiv({ children, ...restProps }) {
  return <UserDiv {...restProps}>{children}</UserDiv>;
};

PageHeader.UserGreeting = function PageHeaderNavSpan({
  children,
  ...restProps
}) {
  return <UserGreeting {...restProps}>{children}</UserGreeting>;
};
