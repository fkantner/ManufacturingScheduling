import React, { Component } from 'react'
import './Workcenter.css'
import Machine from './Machine'
import Buffer from '../resources/Buffer'
import Quality from './Quality';

class Workcenter extends Component {
  render() {
    var outputBuffer = Buffer(this.props.wc.OutputBuffer);

    return <div className='workcenter'>
      <div className='workcenter_header'>
        <h4>Workcenter: {this.props.wc.Name}</h4>
      </div>
      <div className='workcenter_body'>
        <Machine machine={this.props.wc.Machine} />
        <Quality inspection={this.props.wc.Inspection} />
        <p>OutputBuffer: { outputBuffer }</p>
      </div>
    </div>
  }
}

export default Workcenter