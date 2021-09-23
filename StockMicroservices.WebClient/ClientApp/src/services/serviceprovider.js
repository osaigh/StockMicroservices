import { StockAPIConfig } from "../configuration/config";
import UserAuthenticationService from "./user-authentication-service";
import StockApiService from "./stock-api-service";

export const getUserAuthenticationService = () => {
  return new UserAuthenticationService();
};

export const getStockApiService = () => {
  return new StockApiService(StockAPIConfig.baseURL);
};
