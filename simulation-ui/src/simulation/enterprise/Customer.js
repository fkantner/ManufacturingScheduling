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
  ontime(due, completion) {
    return completion <= due;
  }
  
  otif(activelist, completelist, type, day) {
    const openorders = (day === null || type === null) ? activelist.length : activelist.filter(x => x.due === day && x.type === type).length;
    const closedorders = (day === null || type === null) ? completelist : completelist.filter(x => x.due === day && x.type === type);
    const ontime = closedorders.filter( x => this.ontime(x.due, x.complete) ).length;
  
    if (openorders + closedorders.length === 0) { return 0 }
  
    return (ontime + 0.0) / (openorders + closedorders.length);
  }

  render () {
    const activeOrders = this.props.customer.ActiveOrders.map(function(order) { return CreateOrderObject(order); });
    const completeOrders = this.props.customer.CompleteOrders.map(function(order) { return CreateOrderObject(order); });
    const that = this;

    const fullotif = that.otif(activeOrders, completeOrders, null, null).toFixed(3);

    const setup = days.map(function(dayTxt, dayIdx){
      const types = activeOrders.map((x) => x.type).concat(completeOrders.map((x) => x.type)).sort();
      const distinctTypes = [...new Set(types)];

      const obj = distinctTypes.map((type) => { 
        const ootif = that.otif(activeOrders, completeOrders, type, dayIdx).toFixed(1);
        return {
          type: type,
          active: filteredCount(type, dayIdx, activeOrders),
          complete: filteredCount(type, dayIdx, completeOrders),
          otif: ootif
        }
      });

      return { day: dayTxt, obj: obj };
    });

    return (
      <div key="customer" className="customer">
        <h2>Customer <span className="otif">{fullotif}%</span></h2>

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
                      <div>{lowerObj.otif}%</div>
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