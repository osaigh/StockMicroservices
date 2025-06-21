import { UserManager, Log, User } from "oidc-client";
import { IdentityConfig } from "../configuration/config";

export default class UserAuthenticationService {
  UserManager: UserManager;
  currentuser: User | null;

  constructor() {
    IdentityConfig.authority = window._env_.AUTHORITY;
    IdentityConfig.redirect_uri = window._env_.REDIRECT_URI;
    IdentityConfig.post_logout_redirect_uri = window._env_.POST_LOGOUT_REDIRECT_URI;
    IdentityConfig.metadata.issuer = window._env_.ISSUER;
    IdentityConfig.metadata.authorization_endpoint = window._env_.AUTHORIZATION_ENDPOINT;
    IdentityConfig.metadata.jwks_uri = window._env_.JWKS_URI;
    IdentityConfig.metadata.check_session_iframe = window._env_.CHECK_SESSION_IFRAME;
    IdentityConfig.metadata.revocation_endpoint = window._env_.REVOCATION_ENDPOINT;
    IdentityConfig.metadata.introspection_endpoint = window._env_.INTROSPECTION_ENDPOINT;
    IdentityConfig.metadata.device_authorization_endpoint = window._env_.DEVICE_AUTHORIZATION_ENDPOINT;
    IdentityConfig.metadata.userinfo_endpoint = window._env_.USERINFO_ENDPOINT;
    IdentityConfig.metadata.token_endpoint = window._env_.TOKEN_ENDPOINT;
    IdentityConfig.metadata.end_session_endpoint = window._env_.END_SESSION_ENDPOINT;
    this.UserManager = new UserManager(IdentityConfig);
    Log.logger = console;
    Log.level = Log.DEBUG;
    this.currentuser = null;
    this.UserManager.events.addUserLoaded(this.userLoadedListener);
    this.UserManager.events.addUserUnloaded(this.userUnloadedListener);
    this.UserManager.events.addUserSignedOut(this.userSignedOutListener);
  }

  signIn = () => {
    //alert("SignIn called");
    this.UserManager.signinRedirect();
  };

  signOut = () => {
    alert("SignOut called");
    this.currentuser = null;
    this.UserManager.signoutRedirect();
  };

  userLoadedListener = (user: User|null) => {
    console.log("User signed in");
    this.currentuser = user;
  };

  userUnloadedListener = () => {
    console.log("userUnloadedListener User signed out");
    this.currentuser = null;
  };

  userSignedOutListener = () => {
    this.currentuser = null;
    console.log("userSignedOutListener User signed out");
  };

  getUser = async () => {
    const user = await this.UserManager.getUser();
    this.currentuser = user;
    return user;
    };

  getUserIdentity = async () => {
      const user = await this.UserManager.getUser();
      return user;
  };

  signinRedirectCallback = async () => {
    await this.UserManager.signinRedirectCallback();
  };

  signoutRedirectCallback = async () => {
    await this.UserManager.signoutRedirectCallback();
  };

  getCurrentUser() {
    return this.currentuser;
  }
}
