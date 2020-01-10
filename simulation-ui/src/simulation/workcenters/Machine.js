import React, { Component } from 'react'
import Workorder from '../resources/Workorder'
import './Machine.css'
import Buffer from '../resources/Buffer'

class Machine extends Component {
  render() {
    const buffer = this.props.machine.InputBuffer;
    var inputbuffer = Buffer("Input Buffer", buffer);

    return (<div className='machine'>
      <div className='machine_header'>
        <h4>Machine Type: {this.props.machine.Name}</h4>
      </div>
      <div className='machine_body'>

        { inputbuffer }

        <div className='machine_data'>
          <h4>Operation</h4>
          <div>Setup Time: {this.props.machine.SetupTime}</div>
          <div>Est Time to Complete: {this.props.machine.EstTimeToComplete}</div>
          <div>Last Op Type: {this.props.machine.LastType}</div>
        </div>
        
        <div className='machine_current_wo'>
          <h4>Current Workorder</h4>
          <Workorder workorder={this.props.machine.CurrentWorkorder} />
        </div>
      </div>
      
    </div>);
  }
}

export default Machine