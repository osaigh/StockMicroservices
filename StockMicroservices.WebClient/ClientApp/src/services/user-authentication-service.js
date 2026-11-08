import { UserManager, Log } from "oidc-client";
import { IdentityConfig } from "../configuration/config";

export default class UserAuthenticationService {
  constructor() {
    this.UserManager = new UserManager(IdentityConfig);
    Log.logger = console;
    Log.level = Log.WARN;
    this.currentuser = null;

    this.UserManager.events.addUserLoaded(this.userLoadedListener);
    this.UserManager.events.addUserUnloaded(this.userUnloadedListener);
    this.UserManager.events.addUserSignedOut(this.userSignedOutListener);
  }

  signIn = () => {
    return this.UserManager.signinRedirect();
  };

  signOut = () => {
    this.currentuser = null;
    return this.UserManager.signoutRedirect();
  };

  userLoadedListener = (user) => {
    this.currentuser = user;
  };

  userUnloadedListener = () => {
    this.currentuser = null;
  };

  userSignedOutListener = () => {
    this.currentuser = null;
  };

  getUser = async () => {
    const user = await this.UserManager.getUser();
    this.currentuser = user;
    return user;
  };

  getUserIdentity = async () => {
    return this.UserManager.getUser();
  };

  signinRedirectCallback = async () => {
    return this.UserManager.signinRedirectCallback();
  };

  signoutRedirectCallback = async () => {
    return this.UserManager.signoutRedirectCallback();
  };

  getCurrentUser() {
    return this.currentuser;
  }
}
