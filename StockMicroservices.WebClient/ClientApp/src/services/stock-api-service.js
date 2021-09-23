import axios from "axios";

export default class StockApiService {
  baseURL;

  constructor(_baseURL) {
    this.baseURL = _baseURL;
  }

  getAllStocks = () => {
    return axios.get(this.baseURL + "stock");
  };

  create = (newObject) => {
    return axios.post(this.baseURL, newObject);
  };

  update = (id, newObject) => {
    return axios.put(`${this.baseURL}/${id}`, newObject);
  };

  getStockHistory = (stockId) => {
    return axios.get(this.baseURL + "stockHistory/" + stockId);
  };
}
