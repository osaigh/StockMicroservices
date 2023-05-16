import React, { useContext, useEffect, useState } from "react";
import { Button, Card, Table, Container, Row, Col } from "react-bootstrap";
import Chart from '../../components/chart/Chart';
import moment from "moment";
import { StockApiContext } from "../../context/stock-api-context";
import { UserAuthenticationContext } from "../../context/user-manager-context";
import "./stockcontainer.scss"
import "bootstrap/dist/css/bootstrap.min.css";



export function StockContainer() {
  const [lineChartData, setLineChartData] = useState({ title: "", data: [] });
    const [stocks, setStocks] = useState({user:null, stockData:[]});
  const { stockApiService } = useContext(StockApiContext);
  const { userAuthenticationService } = useContext(UserAuthenticationContext);

  useEffect(() => {
    const interval = setInterval(async () => {
      let _user =await userAuthenticationService.getUserIdentity();
      if(_user){
          stockApiService.getAllStocks(_user.access_token).then((response) => {
              console.log(response.data);
              setStocks({ user: _user, stockData: response.data });
          });
      }
      
    }, 5000);

    return () => clearInterval(interval);
  }, [stocks]);

  var content = {};

  const getChartData = () => {
    var data = { labels: [], values: [] };

    stocks.stockData.forEach((stock) => {
      data.labels.push(stock.name);
      data.values.push(stock.volume);
    });
  };

  const viewHistory = (stockId) => {
    if (stocks.user == null) {
      return;
     }

    var stock = null;
    stocks.stockData.forEach((st) => {
      if (st.id == stockId) {
        stock = st;
      }
    });

    if (stock == null) {
      return;
    }

    stockApiService.getStockHistory(stockId,stocks.user.access_token).then((response) => {
      var stockHistoryCollection = response.data;
      var newLineChartData = {
        title: stock.name,
        data: stockHistoryCollection,
      };
      setLineChartData(newLineChartData);
    });
  };

  var tableData = stocks.stockData.map((stock, index) => (
    <tr key={index + 1}>
      <td>{index + 1}</td>
      <td>{stock.name}</td>
      <td>${Math.round(stock.price)}</td>
      <td>{stock.volume}</td>
      <td>
        <Button
          className="btn primary"
          onClick={() => {
            viewHistory(stock.id);
          }}
        >
          View History
        </Button>
      </td>
    </tr>
  ));

  var legendItems = stocks.stockData.map((stock, index) => (
    <span>
      <i key={index} className="fas fa-circle text-info"></i>
      {stock.name}
    </span>
  ));

    var data = [];
  var chartData = "";
  if (lineChartData != null && lineChartData.data.length > 0) {
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
                <Chart title="Last 6 Months (Revenue)" aspect={2 / 1} data={data} />
         
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
                                            <th className="border-0">Name</th>
                                            <th className="border-0">Price</th>
                                            <th className="border-0">Volume</th>
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
