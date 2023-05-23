import axios from "axios";

export default class StockApiService {
  baseURL;

  constructor(_baseURL) {
    this.baseURL = _baseURL;
  }

  getAllStocks = (accessToken = "") => {

      if (accessToken.length > 0) {
          const config = {
              headers: { Authorization: `Bearer ${accessToken}` }
          };
          return axios.get(this.baseURL + "stock", config);
      } else {
          return axios.get(this.baseURL + "stock");
      }
    
  };

  create = (newObject) => {
    return axios.post(this.baseURL, newObject);
  };

  update = (id, newObject) => {
    return axios.put(`${this.baseURL}/${id}`, newObject);
  };

  //  getStockHistory = (stockId, accessToken = "") => {
  //      if (accessToken.length > 0) {
  //          const config = {
  //              headers: { Authorization: `Bearer ${accessToken}` }
  //          };
  //          return axios.get(this.baseURL + "stockHistory/" + stockId, config);
  //      } else {
  //          return axios.get(this.baseURL + "stockHistory/" + stockId);
  //      }
    
  //};
}
