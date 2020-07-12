import React, { Component } from 'react';
import './Customer.css';

function Due(props) {
  return props.Due;
} 

function Complete(props) {
  return props.Complete;
}

function Type(props) {
  return props.Type;
}

class Customer extends Component {
  render () {
    const activeOrders = this.props.customer.ActiveOrders;
    const completeOrders = this.props.customer.CompleteOrders;

    return (
      <div key="customer" className="customer">
        <h2>Customer</h2>
        <ul className="activeOrders">
          {activeOrders.map((order, i) => {
            return (
              <li key={"ActiveOrder" + i} className="Order">
                {Type(order)} {Due(order)}
              </li>
            )
          })}
        </ul>

        <ul className="completeOrders">
          {completeOrders.map((order, i) => {
            return (
              <li key={"CompleteOrder" + i} className="Order">
                {Type(order)} {Due(order)} {Complete(order)}
              </li>
            )
          })}
        </ul>

      </div>
    );
  }
}

export default Customer;