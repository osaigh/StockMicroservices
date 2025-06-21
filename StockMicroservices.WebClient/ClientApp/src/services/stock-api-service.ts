import axios, { AxiosResponse } from "axios";
import { Stock, StockHistory, StockOrder, StockPosition } from "../models";

export default class StockApiService {
  baseURL:string;

  constructor(_baseURL:string) {
    this.baseURL = _baseURL;
  }

  getAllStocks = (accessToken = ""):Promise<AxiosResponse<Stock[]>> => {
      if (accessToken.length > 0) {
          const config = {
              headers: { Authorization: `Bearer ${accessToken}` }
          };
          return axios.get<Stock[]>(this.baseURL + "stock", config);
      } else {
          return axios.get<Stock[]>(this.baseURL + "stock");
      }
    
  };

  getAllStockPositions = (accessToken = ""):Promise<AxiosResponse<StockPosition[]>> => {
    if (accessToken.length > 0) {
        const config = {
            headers: { Authorization: `Bearer ${accessToken}` }
        };
        return axios.get<StockPosition[]>(this.baseURL + "stockPosition", config);
    } else {
        return axios.get<StockPosition[]>(this.baseURL + "stockPosition");
    }
  
};

    getAllStockOrders = (accessToken = ""):Promise<AxiosResponse<StockOrder[]>> => {

    if (accessToken.length > 0) {
        const config = {
            headers: { Authorization: `Bearer ${accessToken}` }
        };
        return axios.get<StockOrder[]>(this.baseURL + "stockorder", config);
    } else {
        return axios.get<StockOrder[]>(this.baseURL + "stockorder");
    }
  
};

  createStockOrder = (stockOrder:StockOrder,accessToken = ""):Promise<AxiosResponse<StockOrder>> =>{
    if (accessToken.length > 0) {
        const config = {
            headers: { 
                Authorization: `Bearer ${accessToken}`, 
                'Accept': 'application/json',
                'Content-Type': 'application/json' 
            }
        };
        return axios.post<StockOrder>(this.baseURL + "stockorder", stockOrder, config);
    } else {
        return axios.post<StockOrder>(this.baseURL + "stockorder", stockOrder);
    }
  }

  getStockHistory = (stockId:number, accessToken = "") :Promise<AxiosResponse<StockHistory[]>>=> {
        if (accessToken.length > 0) {
            const config = {
                headers: { Authorization: `Bearer ${accessToken}` }
            };
            return axios.get(this.baseURL + "stockhistory/" + stockId, config);
        } else {
            return axios.get(this.baseURL + "stockhistory/" + stockId);
        }
    
  };
}
