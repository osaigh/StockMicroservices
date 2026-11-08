import axios from "axios";

export default class StockApiService {
  constructor(baseURL) {
    this.baseURL = baseURL;
  }

  getAllStocks = (accessToken) => {
    const config = accessToken
      ? { headers: { Authorization: `Bearer ${accessToken}` } }
      : undefined;

    return axios.get(`${this.baseURL}stock`, config);
  };
}
