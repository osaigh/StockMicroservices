import React from 'react';
import './chart.scss'
import {
    AreaChart,
    Area,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    ResponsiveContainer,
  } from "recharts";

const toDollars = (decimal) => `${(decimal).toFixed(0)}$`;


const Chart = ({ aspect, title, data }) => {
    console.log(data);
    return (
        <div className="chart">
      <div className="title">{title}</div>
      <div className='ct'>

        <ResponsiveContainer width="100%">
            <AreaChart 
            data={data}
            margin={{ top: 10, right: 30, left: 0, bottom: 0 }}
            >
            <defs>
                <linearGradient id="price" x1="0" y1="0" x2="0" y2="1">
                <stop offset="5%" stopColor="#8884d8" stopOpacity={0.8} />
                <stop offset="95%" stopColor="#8884d8" stopOpacity={0} />
                </linearGradient>
            </defs>
            <XAxis dataKey="date" stroke="gray" />
            <YAxis dataKey="price" tickFormatter={toDollars} />
            <CartesianGrid strokeDasharray="3 3" className="chartGrid" />
            <Tooltip />
            <Area
                type="monotone"
                dataKey="price"
                stroke="#8884d8"
                fillOpacity={1}
                fill="url(#price)"
            />
            </AreaChart>
        </ResponsiveContainer>
      </div>
    </div>
    );
};

export default Chart;