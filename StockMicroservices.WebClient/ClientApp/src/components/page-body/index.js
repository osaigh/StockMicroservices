import React from "react";
import { DivContainer } from "./styles/page-body";

export default function PageBody({ children, ...restProps }) {
  return <DivContainer {...restProps}>{children}</DivContainer>;
}
