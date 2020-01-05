import React, { Component } from 'react'
import Workorder from '../resources/Workorder'
import './Machine.css'
import Buffer from '../resources/Buffer'

class Machine extends Component {
  render() {
    const buffer = this.props.machine.InputBuffer;
    var inputbuffer = Buffer(buffer);

    return (<div className='machine'>
      <div class='machine_header'>
        <h4>Machine Type: {this.props.machine.Name}</h4>
      </div>
      <div class='machine_body'>

        <div class='buffer'><h5>Input Buffer</h5>
          { inputbuffer }
        </div>

        <div class='machine_data'>
          <div>Setup Time: {this.props.machine.SetupTime}</div>
          <div>Est Time to Complete: {this.props.machine.EstTimeToComplete}</div>
          <div>Last Op Type: {this.props.machine.LastType}</div>
        </div>
        
        <div class='machine_current_wo'>
          <p>Current Workorder: </p><Workorder workorder={this.props.machine.CurrentWorkorder} />
        </div>
      </div>
      
    </div>);
  }
}

export default Machine