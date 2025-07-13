import React from 'react';

const Chart: React.FC = () => {
  const data = [
    { month: 'Jan', value: 30 },
    { month: 'Feb', value: 45 },
    { month: 'Mar', value: 35 },
    { month: 'Apr', value: 55 },
    { month: 'May', value: 65 },
    { month: 'Jun', value: 48 },
    { month: 'Jul', value: 70 },
  ];

  const maxValue = Math.max(...data.map(d => d.value));

  return (
    <div className="space-y-4">
      <div className="flex justify-between items-end h-48 space-x-2">
        {data.map((item, index) => (
          <div key={item.month} className="flex flex-col items-center flex-1">
            <div className="relative w-full flex items-end justify-center">
              <div
                className="bg-gradient-to-t from-blue-500 to-blue-400 rounded-t-md w-8 transition-all duration-500 ease-out hover:from-blue-600 hover:to-blue-500 cursor-pointer"
                style={{
                  height: `${(item.value / maxValue) * 160}px`,
                  animationDelay: `${index * 100}ms`
                }}
                title={`${item.month}: ${item.value}%`}
              />
            </div>
            <span className="text-xs text-gray-500 mt-2 font-medium">{item.month}</span>
          </div>
        ))}
      </div>
      
      <div className="flex justify-between text-xs text-gray-400 border-t pt-2">
        <span>0%</span>
        <span>25%</span>
        <span>50%</span>
        <span>75%</span>
        <span>100%</span>
      </div>
    </div>
  );
};

export default Chart;