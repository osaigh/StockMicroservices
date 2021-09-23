import Oidc from "oidc-client";

export const IdentityConfig = {
  userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
  authority: "https://localhost:44313/",
  client_id: "client_id_react2",
  response_type: "id_token token",
  redirect_uri: "https://localhost:3000/signincallback",
  post_logout_redirect_uri: "https://localhost:3000/signoutcallback",
  scope: "openid profile email StockMicroservicesAPI",
  metadata: {
    issuer: "https://localhost:44313",
    jwks_uri: "https://localhost:44313/.well-known/openid-configuration/jwks",
    authorization_endpoint: "https://localhost:44313/connect/authorize",
    token_endpoint: "https://localhost:44313/connect/token",
    userinfo_endpoint: "https://localhost:44313/connect/userinfo",
    end_session_endpoint: "https://localhost:44313/connect/endsession",
    check_session_iframe: "https://localhost:44313/connect/checksession",
    revocation_endpoint: "https://localhost:44313/connect/revocation",
    introspection_endpoint: "https://localhost:44313/connect/introspect",
    device_authorization_endpoint:
      "https://localhost:44313/connect/deviceauthorization",
  },
};

export const StockAPIConfig = {
    baseURL: "https://localhost:44324/",
};
