import { useState, useContext, useEffect } from "react";
import { Card, Table, Container, Row, Col } from "react-bootstrap";
import Chart from "../../components/chart/Chart";
import moment from "moment";
import { StockApiContext } from "../../context/stock-api-context";
import { UserAuthenticationContext } from "../../context/user-manager-context";
import "./stockcontainer.scss";
import "bootstrap/dist/css/bootstrap.min.css";

const emptyState = { user: null, stockData: [] };

export function StockContainer() {
  const [lineChartData, setLineChartData] = useState(null);
  const [stocks, setStocks] = useState(emptyState);
  const [error, setError] = useState(null);
  const { stockApiService } = useContext(StockApiContext);
  const { userAuthenticationService } = useContext(UserAuthenticationContext);

  useEffect(() => {
    let cancelled = false;

    const loadStocks = async () => {
      try {
        const user = await userAuthenticationService.getUserIdentity();
        if (!user) {
          if (!cancelled) {
            setStocks(emptyState);
          }
          return;
        }

        const response = await stockApiService.getAllStocks(user.access_token);
        if (!cancelled) {
          setStocks({ user, stockData: response.data });
          setError(null);
        }
      } catch (requestError) {
        if (!cancelled) {
          setError("Unable to load stock data. Check that the API is running.");
        }
      }
    };

    loadStocks();
    const interval = setInterval(loadStocks, 5000);

    return () => {
      cancelled = true;
      clearInterval(interval);
    };
  }, [stockApiService, userAuthenticationService]);

  const viewHistory = (stockId) => {
    if (!stocks.user) {
      return;
    }

    const stock = stocks.stockData.find((item) => item.id === stockId);
    if (!stock) {
      return;
    }

    setLineChartData({
      title: stock.name,
      data: stock.stockHistories || [],
    });
  };

  const tableData = stocks.stockData.map((stock, index) => (
    <tr key={stock.id || index}>
      <td>{index + 1}</td>
      <td>{stock.name}</td>
      <td>${Math.round(stock.price)}</td>
      <td>{stock.volume ?? 0}</td>
      <td>
        <button
          type="button"
          className="btn btn-primary"
          onClick={() => viewHistory(stock.id)}
        >
          View History
        </button>
      </td>
    </tr>
  ));

  let chartData = null;
  if (lineChartData && lineChartData.data.length > 0) {
    const data = lineChartData.data.map((history) => ({
      date: moment(history.date).format("L"),
      price: history.price,
    }));

    chartData = (
      <Card className="bg">
        <Card.Header>
          <Card.Title as="h4">{lineChartData.title}</Card.Title>
        </Card.Header>
        <Card.Body>
          <Chart title="Price history" data={data} />
        </Card.Body>
        <Card.Footer>
          <div className="legend">
            <i className="fas fa-circle text-primary"></i>
            History
          </div>
        </Card.Footer>
      </Card>
    );
  }

  return (
    <div className="container-fluid">
      <Container fluid>
        <Row>
          <Col md="8">
            <Card className="strpied-tabled-with-hover bg">
              <Card.Header>
                <Card.Title as="h4">Stocks</Card.Title>
                <p className="card-category">Stock prices and volume</p>
              </Card.Header>
              <Card.Body className="table-full-width table-responsive px-0">
                {error ? <p className="text-danger">{error}</p> : null}
                <Table className="table-hover table-striped">
                  <thead>
                    <tr>
                      <th className="border-0">#</th>
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
    </div>
  );
}
