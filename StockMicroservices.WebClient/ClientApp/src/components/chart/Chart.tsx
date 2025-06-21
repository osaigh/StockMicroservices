import './chart.scss';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

const toDollars = (decimal: any) => `$${(decimal).toFixed(0)}`;

export interface ChartProps{
    title:string,
    data:any[]
}
const Chart = ({ title, data }: ChartProps) => {
    return (
        <div className="chart">
      <div className="title">{title}</div>
      <div className='ct'>

        <ResponsiveContainer width="100%">
            <LineChart
              width={500}
              height={300}
              data={data}
              margin={{ top: 10, right: 30, left: 0, bottom: 0 }}
            >
              <CartesianGrid strokeDasharray="3 3" className="chartGrid" />
              <XAxis dataKey="date" stroke="gray" />
              <YAxis dataKey="price" tickFormatter={toDollars} />
              <Tooltip />
              <Legend />
              <Line type="monotone" dataKey="price" stroke="#8884d8" activeDot={{ r: 8 }} />
              
            </LineChart>
            
        </ResponsiveContainer>
      </div>
    </div>
    );
};

export default Chart;