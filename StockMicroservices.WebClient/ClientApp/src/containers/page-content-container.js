import React from "react";

export function PageContentContainer({ children }) {
  return (
    <div className="container-fluid">
      <div className="row">{children}</div>
    </div>
  );
}
