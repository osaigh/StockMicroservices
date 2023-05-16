import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import { Provider } from 'react-redux';
import {store} from './store';
import { UserAuthenticationContext } from "./context/user-manager-context";
import { StockApiContext } from "./context/stock-api-context";
import {
    getUserAuthenticationService,
    getStockApiService,
} from "./services/serviceprovider";
import './index.css';


const userAuthenticationService = getUserAuthenticationService();
const stockApiService = getStockApiService();
const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <>
    <UserAuthenticationContext.Provider value={{ userAuthenticationService }}>
      <StockApiContext.Provider value={{ stockApiService }}>
        <Provider store={store}>
          <App />
        </Provider>
      </StockApiContext.Provider>
    </UserAuthenticationContext.Provider>
  </>
  );
