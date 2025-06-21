import { createContext } from "react";
import { UserAuthenticationService } from "../services";

export const UserAuthenticationContext = createContext<UserAuthenticationService|null>(null);