import React, { Component } from 'react';
import './Customer.css';

function CreateOrderObject(order) {
  const set = order.split(';').map(function(part){ return part.trim(); });
  const complete = set.length > 2 ? set[2] : null;
  return {
    type: set[0],
    due: parseInt(set[1]),
    complete: parseInt(complete)
  };
}

const days = [
  'Sun',
  'Mon',
  'Tue',
  'Wed',
  'Thu', 
  'Fri',
  'Sat'
];

function filteredCount(type, day, list) {
  return list.filter(x => x.due === day && x.type === type).length;
} 

class Customer extends Component {
  

  render () {
    const activeOrders = this.props.customer.ActiveOrders.map(function(order) { return CreateOrderObject(order); });
    const completeOrders = this.props.customer.CompleteOrders.map(function(order) { return CreateOrderObject(order); });
    
    const setup = days.map(function(dayTxt, dayIdx){
      const types = activeOrders.map((x) => x.type).concat(completeOrders.map((x) => x.type));
      const distinctTypes = [...new Set(types)];

      const obj = distinctTypes.map((type) => { 
        return {
          type: type,
          active: filteredCount(type, dayIdx, activeOrders),
          complete: filteredCount(type, dayIdx, completeOrders)
        }
      });

      return { day: dayTxt, obj: obj };
    });

    return (
      <div key="customer" className="customer">
        <h2>Customer</h2>

        <ul className="customerDays">
          {setup.map((obj, index) => {
            return (
              <li className="customerDay" key={"customerDay" + index}>
                <h6>{obj.day}</h6>
                <ol className="customerTypes">
                  {obj.obj.map((lowerObj, indexj) => {
                    return (<li key={"customerType" + lowerObj.type + index + indexj}>
                      <div>{lowerObj.type}</div>
                      <div>{lowerObj.complete}/{lowerObj.active + lowerObj.complete}</div>
                    </li>
                    );
                  })}
                </ol>
              </li>
            );
          })}
        </ul>
      </div>
    );
  }
}

export default Customer;