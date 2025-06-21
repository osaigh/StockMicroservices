import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import { Provider } from 'react-redux';
import {store} from './store';
import { UserAuthenticationContext,StockApiContext } from './context';
import { StockApiService, UserAuthenticationService } from './services';
import { StockAPIConfig } from './configuration/config';

fetch('/config.json')
  .then((res) => res.json())
  .then((config) => {
    window._env_ = config;
    const userAuthenticationService = new UserAuthenticationService();
    StockAPIConfig.baseURL = window._env_.STOCK_API;
    const stockApiService =new StockApiService(StockAPIConfig.baseURL);
    const root = ReactDOM.createRoot(document.getElementById('root')  as HTMLElement);
    root.render(
      <>
        <UserAuthenticationContext.Provider value={userAuthenticationService}>
          <StockApiContext.Provider value={stockApiService }>
            <Provider store={store}>
              <App />
            </Provider>
          </StockApiContext.Provider>
        </UserAuthenticationContext.Provider>
      </>
      );
      })
  .catch((err) => {
    console.error('Failed to load config:', err);
  });

