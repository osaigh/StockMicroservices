import Oidc from "oidc-client";

export const IdentityConfig = {
  userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
    authority: "https://localhost:44401/",
  client_id: "client_id_react",
  response_type: "id_token token",
    redirect_uri: "http://localhost:44100/signincallback",
    post_logout_redirect_uri: "http://localhost:44100/signoutcallback",
  scope: "openid  profile ApiOne ApiTwo StockAPI",
  metadata: {
      issuer: "https://localhost:44401",
      jwks_uri: "https://localhost:44401/.well-known/openid-configuration/jwks",
      authorization_endpoint: "https://localhost:44401/connect/authorize",
      token_endpoint: "https://localhost:44401/connect/token",
      userinfo_endpoint: "https://localhost:44401/connect/userinfo",
      end_session_endpoint: "https://localhost:44401/connect/endsession",
      check_session_iframe: "https://localhost:44401/connect/checksession",
      revocation_endpoint: "https://localhost:44401/connect/revocation",
      introspection_endpoint: "https://localhost:44401/connect/introspect",
    device_authorization_endpoint:
      "https://localhost:44401/connect/deviceauthorization",
  },
};

export const StockAPIConfig = {
    baseURL: "http://localhost:44405/api/",
};
