import React, { useContext, useEffect, useState } from "react";
import { Button, Card, Table, Container, Row, Col } from "react-bootstrap";
import ChartistGraph from "react-chartist";
import moment from "moment";
import { StockApiContext } from "../context/stock-api-context";

export function StockContainer() {
  const [lineChartData, setLineChartData] = useState({ title: "", data: [] });
  const [stocks, setStocks] = useState([]);
  const { stockApiService } = useContext(StockApiContext);

  useEffect(() => {
    const interval = setInterval(() => {
      stockApiService.getAllStocks().then((response) => {
        console.log(response.data);
        setStocks(response.data);
      });
    }, 5000);

    return () => clearInterval(interval);
  }, [stocks]);

  var content = {};

  const getChartData = () => {
    var data = { labels: [], values: [] };

    stocks.forEach((stock) => {
      data.labels.push(stock.name);
      data.values.push(stock.volume);
    });
  };

  const viewHistory = (stockId) => {
    var stock = null;
    stocks.forEach((st) => {
      if (st.id == stockId) {
        stock = st;
      }
    });
    if (stock == null) {
      return;
    }
    stockApiService.getStockHistory(stockId).then((response) => {
      var stockHistoryCollection = response.data;
      var newLineChartData = {
        title: stock.name,
        data: stockHistoryCollection,
      };
      setLineChartData(newLineChartData);
    });
  };

  var tableData = stocks.map((stock, index) => (
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

  var legendItems = stocks.map((stock, index) => (
    <span>
      <i key={index} className="fas fa-circle text-info"></i>
      {stock.name}
    </span>
  ));

  var chartData = "";
  if (lineChartData != null && lineChartData.data.length > 0) {
    var chartLabels = [];
    var chartSeries = [];
    var max = 0;
    var min = 200000000;
    lineChartData.data.forEach((stockHistory) => {
      chartLabels.push(moment(stockHistory.date).format("L"));
      chartSeries.push(stockHistory.price);
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
      <Card>
        <Card.Header>
          <Card.Title as="h4">{lineChartData.title}</Card.Title>
        </Card.Header>
        <Card.Body>
          <div className="ct-chart" id="chartHours">
            <ChartistGraph
              data={{
                labels: chartLabels,
                series: [chartSeries],
              }}
              type="Line"
              options={{
                low: min,
                high: max,
                showArea: false,
                height: 200,
                axisX: {
                  showGrid: true,
                },
                lineSmooth: true,
                showLine: true,
                showPoint: true,
                fullWidth: true,
                chartPadding: {
                  right: 50,
                },
              }}
              responsiveOptions={[
                [
                  "screen and (max-width: 640px)",
                  {
                    axisX: {
                      labelInterpolationFnc: function (value) {
                        return value[0];
                      },
                    },
                  },
                ],
              ]}
            />
          </div>
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
            <Card className="strpied-tabled-with-hover">
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
