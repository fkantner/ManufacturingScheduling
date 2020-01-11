import React, { Component } from 'react';
import './Dock.css';
import Buffer from '../resources/Buffer';

class Dock extends Component {
  render() {
    var outputBuffer = Buffer("Output Buffer:", this.props.dock.OutputBuffer);
    var shippingBuffer = Buffer("Shipping Buffer:", this.props.dock.ShippingBuffer);

    return <div className='dock'>
      <div className='dock_header'>
        <h4>{this.props.dock.Name}</h4>
      </div>
      <div className='dock_body'>
        { outputBuffer }
        { shippingBuffer }
      </div>
    </div>
  }
}

export default Dock