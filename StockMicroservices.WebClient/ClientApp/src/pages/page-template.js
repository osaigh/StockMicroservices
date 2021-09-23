import React from "react";
import { PageContainer } from "../containers/page-container";
import { PageHeaderContainer } from "../containers/header";
import { PageBodyContainer } from "../containers/page-body-container";
import { PageContentContainer } from "../containers/page-content-container";
import { PageSideBarContainer } from "../containers/page-side-bar-container";

export default function PageTemplate({
  children,
  sidebarlinks,
  signIn,
  signOut,
  userGreeting,
  isUserSignedIn,
}) {
  return (
    <PageContainer>
      <PageHeaderContainer
        signIn={signIn}
        signOut={signOut}
        userGreeting={userGreeting}
        isUserSignedIn={isUserSignedIn}
      ></PageHeaderContainer>
      <PageBodyContainer>
        <PageSideBarContainer>{sidebarlinks}</PageSideBarContainer>
        <PageContentContainer>{children}</PageContentContainer>
      </PageBodyContainer>
    </PageContainer>
  );
}
