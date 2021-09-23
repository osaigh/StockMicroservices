import Oidc from "oidc-client";
import { IdentityConfig } from "../configuration/config";

var UserManager = new Oidc.UserManager(IdentityConfig);
UserManager.getUser().then((user) => {
  console.log("user:", user);
  return user;
});

var SignIn = function () {
  alert("SignIn called");
  UserManager.signinRedirect();
};

var SignOut = function () {
  alert("SignOut called");

  UserManager.signoutRedirect();
};

const userManagerService = {
  userManager: UserManager,
  signIn: SignIn,
  signOut: SignOut,
};

export { userManagerService };
