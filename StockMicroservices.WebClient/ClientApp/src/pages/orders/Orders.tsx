import {useState,useContext,useEffect} from "react";
import { UserAuthenticationContext } from "../../context";
import { StockApiContext } from "../../context/stock-api-context";
import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardContent from '@mui/material/CardContent';
import Datatable from "../../components/datatable/Datatable";
import PageTemplate from "../templates/page-template";
import { StockOrder } from "../../models";
import { StockApiService, UserAuthenticationService } from "../../services";
import { User } from "oidc-client";

const getTimeInForceString = (timeInForce:string) =>{
    switch(timeInForce){
        case "EndOfDay": return 'End of Day';
        case "ThirtyDays": return 'Thirty Days';
    }

    return '';
}

const getTableData = (stockOrders: StockOrder[]) =>{
    if(!stockOrders){
        return [];
    }

    const tableData: any[] = [];
    stockOrders.forEach((stockOrder, index) => {
        tableData.push(
            {
                id: index+1,
                stock: stockOrder.stock,
                transactionType: stockOrder.transactionType,
                shares: `${stockOrder.shares}`,
                priceLimit: `$${stockOrder.stopLimitPrice}`,
                orderType: stockOrder.orderType,
                timeInForce: getTimeInForceString(stockOrder.timeInForce),
            }
        );
    });

    return tableData;
}

const columns = [
    { field: "id", headerName: "ID", width: 70 },
    {
      field: "stock",
      headerName: "Stock",
      width: 100,
    },
    {
      field: "transactionType",
      headerName: "Transaction",
      width: 100,
    },
    {
        field: "shares",
        headerName: "Shares",
        width: 100,
      },
    {
      field: "priceLimit",
      headerName: "Price Limit",
      width: 150,
    },
    {
      field: "orderType",
      headerName: "Order Type",
      width: 200,
    },
    {
      field: "timeInForce",
      headerName: "Term",
      width: 200,
    },
  ];

function Orders() {
    const userAuthenticationService = useContext<UserAuthenticationService|null>(UserAuthenticationContext);  
    const [stockOrders, setStockOrders] = useState<StockOrder[]>([]);
    const stockApiService = useContext<StockApiService|null>(StockApiContext);
    const [user, setUser] = useState<User|null>(null);

          useEffect(() => {
              async function getUserAsync() {
                const user = await userAuthenticationService?.getUser();
                return user;
              }
          
              getUserAsync().then((user) => {
                if(user){
                  setUser(user);
                }
              });
            }, []);
    useEffect(() => {
        if(user){
            stockApiService?.getAllStockOrders(user.access_token).then((response) => {
                setStockOrders(response.data);
            });
        }
      }, [user]);

      const tableData = getTableData(stockOrders);
    
      return (
        <PageTemplate>
            <Card sx={{width: "100%", }}>
                <CardHeader title="Stock Orders">
                
                </CardHeader>
                <CardContent>
                    <Datatable height="400px" rowData={tableData} columns={columns}>

                    </Datatable>
                </CardContent>
            </Card>
        </PageTemplate>
      )
}

export default Orders