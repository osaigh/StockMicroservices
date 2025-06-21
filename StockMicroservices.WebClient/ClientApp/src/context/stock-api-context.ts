import { createContext } from "react";
import { StockApiService } from "../services";

export const StockApiContext = createContext<StockApiService|null>(null);