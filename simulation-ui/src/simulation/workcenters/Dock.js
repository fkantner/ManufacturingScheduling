import { Component } from 'react';
import './Dock.css';
import Buffer from '../resources/Buffer';
import AcceptWorkorder from './AcceptWorkorder';

class Dock extends Component {
  render() {
    const outputBuffer = Buffer("Output Buffer:", this.props.dock.OutputBuffer);
    const shippingBuffer = Buffer("Shipping Buffer:", this.props.dock.ShippingBuffer);

    const options = {
      name: 'dock',
      title: this.props.dock.Name,
      body: [
        outputBuffer,
        shippingBuffer
      ]
    };

    return AcceptWorkorder(options);
  }
}

export default Dock