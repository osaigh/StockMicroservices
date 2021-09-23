import React from "react";
import ReactDOM from "react-dom";
import App from "./App";
import { GlobalStyles } from "./global-styles";
import { UserAuthenticationContext } from "./context/user-manager-context";
import { StockApiContext } from "./context/stock-api-context";
import {
    getUserAuthenticationService,
    getStockApiService,
} from "./services/serviceprovider";
import "bootstrap/dist/css/bootstrap.min.css";

const userAuthenticationService = getUserAuthenticationService();
const stockApiService = getStockApiService();
ReactDOM.render(
<>
<UserAuthenticationContext.Provider value={{ userAuthenticationService }}>
<StockApiContext.Provider value={{ stockApiService }}>
<GlobalStyles></GlobalStyles>
    <App />
    </StockApiContext.Provider>
    </UserAuthenticationContext.Provider>
    </>,
document.getElementById("root")
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals