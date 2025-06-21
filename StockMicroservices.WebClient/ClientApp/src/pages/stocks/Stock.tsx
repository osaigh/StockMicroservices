import {useState,useContext,useEffect} from "react";
import { UserAuthenticationContext, StockApiContext } from "../../context";
import Card from '@mui/material/Card';
import CardHeader from '@mui/material/CardHeader';
import CardContent from '@mui/material/CardContent';
import Chart from "../../components/chart/Chart";
import moment from "moment";
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import { Link } from "react-router-dom";
import Datatable from "../../components/datatable/Datatable";
import PageTemplate from "../templates/page-template";
import { StockApiService, UserAuthenticationService } from "../../services";
import { StockHistory, StockPosition } from "../../models";
import { User } from "oidc-client";

interface LineChartData{
    title:string,
    data: StockHistory[]
}

interface ChartDataPoint{
    date:string, price:number
}

const getTableData = (stockPositions:StockPosition[]) =>{
    if(!stockPositions){
        return [];
    }

    const tableData:any = [];
    stockPositions.forEach((stock, index) => {
        tableData.push(
            {
                id: index+1,
                stockId: stock.stockId,
                name: stock.name,
                shares: stock.shares,
                currentPrice: `$${Math.round(stock.currentPrice * 100)/100}`,
                costBasis: `$${Math.round(stock.costBasis * 100)/100}`,
                marketValue: `$${Math.round(stock.marketValue * 100)/100}`,
                gainLossPercent:Math.round(stock.gainLossPercent * 100)/100,
            }
        );
    });

    return tableData;
}

const columns = [
    { field: "id", headerName: "ID", width: 70 },
    {
      field: "name",
      headerName: "Stock",
      width: 100,
    },
    {
      field: "shares",
      headerName: "Shares",
      width: 100,
    },
  
    {
      field: "currentPrice",
      headerName: "Last",
      width: 150,
    },
    {
      field: "costBasis",
      headerName: "Cost Basis",
      width: 200,
    },
    {
      field: "marketValue",
      headerName: "Market Value",
      width: 200,
    },
    {
      field: "gainLossPercent",
      headerName: "Gain/Loss %",
      width: 200,
      renderCell: (params:any) => {
        return (
          <div
            style={{
                padding:"5px",
                borderRadius:"5px",
                backgroundColor: (params.row.gainLossPercent > 0 ? `rgba(0, 128, 0, 0.05)`: (params.row.gainLossPercent === 0? `rgba(255, 217, 0, 0.05)`: `rgba(255, 0, 0, 0.05)`)),
                color: (params.row.gainLossPercent > 0? `green`: (params.row.gainLossPercent === 0 ? `goldenrod`: `crimson`))
            }}
          >
            {params.row.gainLossPercent}
          </div>
        );
      },
    },
  ];

const getChart = (lineChartData:LineChartData) =>{
    let chart = (<div></div>);
    let data:ChartDataPoint[] = [];
    if (lineChartData != null && lineChartData.data.length > 0) {
        let chartLabels = [];
        let chartSeries = [];
        let max = 0;
        let min = 200000000;
      lineChartData.data.forEach((stockHistory) => {
        chartLabels.push(moment(stockHistory.date).format("L"));
          chartSeries.push(stockHistory.price);
          data.push({
              date: moment(stockHistory.date).format("L"),
              price: stockHistory.price
          });
        if (stockHistory.price > max) {
          max = stockHistory.price;
        }
        if (stockHistory.price < min) {
          min = stockHistory.price;
        }
      });
  
      max = Math.ceil(max);
      min = Math.floor(min) - 10;
      if (min < 0) {
        min = 0;
      }
      chart = (
        <Card sx={{width: "100%", height:"330px"}}>
          <CardHeader title={lineChartData.title}>
        
          </CardHeader>
          <CardContent>
                  <Chart title="" data={data} />
          </CardContent>
        </Card>
      );

    }

    return chart;
}

const Stocks = () => {
    const [user, setUser] = useState<User|null>(null);
    const userAuthenticationService = useContext<UserAuthenticationService|null>(UserAuthenticationContext);   
    const [lineChartData, setLineChartData] = useState<LineChartData>({ title: "", data: [] });
    const [stockPositions, setStockPositions] = useState<StockPosition[]>([]);
    const stockApiService = useContext<StockApiService|null>(StockApiContext);

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
        const interval = setInterval(async () => {
          if(user){
              stockApiService?.getAllStockPositions(user.access_token).then((response) => {
                  setStockPositions(response.data );
              });
          }else{
            setStockPositions([] );
          }
          
        }, 1000);
    
        return () => clearInterval(interval);
      }, [user, stockPositions]);

      const viewHistory = (stockId:number) => {
        if (stockPositions.length == 0) {
          return;
         }
    
        let stock:StockPosition | null = null;
        stockPositions.forEach((st) => {
          if (st.stockId == stockId) {
            stock = st;
          }
        });
    
        if (!stock) {
          return;
        }
    
        stockApiService?.getStockHistory(stockId, user?.access_token).then((response) => {
          var stockHistoryCollection = response.data;
          var newLineChartData: LineChartData = {
            title: stock?.name as string,
            data: stockHistoryCollection,
          };
          setLineChartData(newLineChartData);
        });
      };

      const actionColumn = [
        {
          field: "action",
          headerName: "Action",
          width: 220,
          renderCell: (params:any) => {
            return (
              <Stack direction="row" alignItems="center" gap={2}>
                <Link to={`/order/${params.row.stockId}/buy`} style={{ textDecoration: "none" }}>
                    <Box sx={{
                        padding: '2px 5px',
                        borderRadius: '5px',
                        color: 'darkblue',
                        border: '1px dotted rgba(0, 0, 139, 0.596)',
                        cursor: 'pointer',
                    }}>
                        Buy
                    </Box>
                </Link>
                <Link to={`/order/${params.row.stockId}/sell`} style={{ textDecoration: "none" }}>
                    <Box sx={{
                            padding: '2px 5px',
                            borderRadius: '5px',
                            color: 'crimson',
                            border: '1px dotted rgba(220, 20, 60, 0.6)',
                            cursor: 'pointer',
                        }}>
                        Sell
                    </Box>
                </Link>
                <Box>
                    <Box onClick={()=> viewHistory(params.row.stockId)} sx={{
                            padding: '2px 5px',
                            borderRadius: '5px',
                            color: 'gray',
                            border: '1px dotted gray',
                            cursor: 'pointer',
                        }}>
                        View History
                    </Box> 
                </Box>
              </Stack>
            );
          },
        },
      ];
    
      const tableData =  getTableData(stockPositions);
      
      const chart = getChart(lineChartData);
     
  return (
    <PageTemplate>
        <Card sx={{width: "100%", }}>
          <CardHeader title="Stock Positions">
        
          </CardHeader>
          <CardContent>
            <Datatable height="400px" rowData={tableData} columns={columns.concat(actionColumn)}>

            </Datatable>
          </CardContent>
        </Card>
        
        {chart}
    </PageTemplate>
  )
}

export default Stocks