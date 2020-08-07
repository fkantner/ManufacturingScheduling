import React, { Component } from 'react'
import './Workcenter.css'
import Machine from './Machine'
import Buffer from '../resources/Buffer'
import Quality from './Quality';
import AcceptWorkorder from './AcceptWorkorder';

class Workcenter extends Component {
  render() {
    const outputBuffer = Buffer("Output Buffer:", this.props.wc.OutputBuffer);
    const machine = <Machine machine={this.props.wc.Machine} />
    const inspection = <Quality inspection={this.props.wc.Inspection} />

    const options = {
      name: 'workcenter',
      title: this.props.wc.Name,
      body: [
        machine,
        inspection,
        outputBuffer
      ]
    };

    return AcceptWorkorder(options);
  }
}

export default Workcenter