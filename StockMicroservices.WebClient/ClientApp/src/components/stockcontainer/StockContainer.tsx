import { ReactNode, useContext, useEffect, useState } from "react";
import { Button, Card, Table, Container, Row, Col } from "react-bootstrap";
import Chart from '../../components/chart/Chart';
import moment from "moment";
import { StockApiContext } from "../../context/stock-api-context";
import { UserAuthenticationContext } from "../../context/user-authentication-context";
import "./stockcontainer.scss"
import "bootstrap/dist/css/bootstrap.min.css";
import { StockApiService, UserAuthenticationService } from "../../services";
import { StockPosition } from "../../models";
import { User } from "oidc-client";

const StockContainer = () => {
   const [user, setUser] = useState<User|null>(null);
  const [lineChartData, setLineChartData] = useState<{title:any,data:any[]}>({ title: "", data: [] });
  const [stockPositions, setStockPositions] = useState<StockPosition[]>([]);
  const stockApiService = useContext<StockApiService|null>(StockApiContext);
  const userAuthenticationService = useContext<UserAuthenticationService|null>(UserAuthenticationContext);  

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
      let _user =await userAuthenticationService?.getUser();
      if(_user){
          stockApiService?.getAllStockPositions(_user.access_token).then((response) => {
              console.log(response.data);
              setStockPositions(response.data );
          });
      }
      
    }, 5000);

    return () => clearInterval(interval);
  }, [stockPositions]);

  let content:ReactNode;

  const getChartData = () => {
    let data:{labels:any[],values:any[]} = { labels: [], values: [] };

    stockPositions.forEach((stock) => {
      data.labels.push(stock.stockId);
      data.values.push(stock.shares);
    });
  };

  const viewHistory = (stockId:number) => {
    let stock: StockPosition | null = null;
    stockPositions.forEach((st) => {
      if (st.stockId == stockId) {
        stock = st;
      }
    });

    if (stock == null) {
      return;
    }

    stockApiService?.getStockHistory(stockId, user?.access_token).then((response) => {
      var stockHistoryCollection = response.data;
      var newLineChartData = {
        title: stock?.stockId,
        data: stockHistoryCollection,
      };
      setLineChartData(newLineChartData);
    });
  };

  let tableData = stockPositions ? stockPositions.map((stock, index) => (
    <tr key={index + 1}>
      <td>{index + 1}</td>
      <td>{stock.stockId}</td>
      <td>{stock.shares}</td>
      <td>${Math.round(stock.currentPrice * 100)/100}</td>
      <td>${Math.round(stock.costBasis * 100)/100}</td>
      <td>${Math.round(stock.marketValue * 100)/100}</td>
      <td>{Math.round(stock.gainLossPercent * 100)/100}</td>
      <td>
        <Button
          className="btn primary"
          onClick={() => {
            viewHistory(stock.stockId);
          }}
        >
          View History
        </Button>
      </td>
    </tr>
  )): [];


  let chartData:any = "";
  if (lineChartData != null && lineChartData.data.length > 0) {
    let data:any[] = [];
    var chartLabels = [];
    var chartSeries = [];
    var max = 0;
    var min = 200000000;
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
    chartData = (
      <Card className="bg">
        <Card.Header>
          <Card.Title as="h4">{lineChartData.title}</Card.Title>
        </Card.Header>
            <Card.Body>
                <Chart title="Last 6 Months (Revenue)" data={data} />
         
        </Card.Body>
        <Card.Footer>
          <div className="legend">
            <i className="fas fa-circle text-primary"></i>
            History
          </div>
          <hr></hr>
          <div className="stats">
            <i className="fas fa-history"></i>
            Updated 3 minutes ago
          </div>
        </Card.Footer>
      </Card>
    );
  }

    content = (
        <>
            <Container fluid>
                <Row>
                    <Col md="8">
                        <Card className="strpied-tabled-with-hover bg">
                            <Card.Header>
                                <Card.Title as="h4">Stocks</Card.Title>
                                <p className="card-category">Stock prices and volume</p>
                            </Card.Header>
                            <Card.Body className="table-full-width table-responsive px-0">
                                <Table className="table-hover table-striped">
                                    <thead>
                                        <tr>
                                            <th className="border-0">ID</th>
                                            <th className="border-0">Stock</th>
                                            <th className="border-0">Shares</th>
                                            <th className="border-0">Last</th>
                                            <th className="border-0">Cost Basis</th>
                                            <th className="border-0">Market Value</th>
                                            <th className="border-0">Gain/Loss %</th>
                                            <th className="border-0"></th>
                                        </tr>
                                    </thead>
                                    <tbody>{tableData}</tbody>
                                </Table>
                            </Card.Body>
                        </Card>
                    </Col>
                    <Col md="4">{chartData}</Col>
                </Row>
            </Container>
        </>
    );

    return <div className="container-fluid">{content}</div>;
}

export default StockContainer;
