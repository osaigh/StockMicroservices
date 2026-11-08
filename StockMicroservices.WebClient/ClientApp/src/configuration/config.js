import Oidc from "oidc-client";

const trimTrailingSlash = (value) => value.replace(/\/+$/, "");

const identityUrl = trimTrailingSlash(
  process.env.REACT_APP_IDENTITY_URL || "http://localhost:44401"
);
const apiUrl = trimTrailingSlash(
  process.env.REACT_APP_API_URL || "http://localhost:44405"
);

export const IdentityConfig = {
  userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
  authority: identityUrl,
  client_id: process.env.REACT_APP_IDENTITY_CLIENT_ID || "client_id_react",
  response_type: "id_token token",
  redirect_uri: `${window.location.origin}/signincallback`,
  post_logout_redirect_uri: `${window.location.origin}/signoutcallback`,
  scope: "openid profile email StockMicroservicesAPI",
};

export const StockAPIConfig = {
  baseURL: `${apiUrl}/`,
};
