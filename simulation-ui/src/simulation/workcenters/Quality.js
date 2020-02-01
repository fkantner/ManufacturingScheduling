import React, { Component } from 'react'
import Workorder from '../resources/Workorder'
import './Quality.css'
import Buffer from '../resources/Buffer'

class Quality extends Component {
  render() {
    const buffer = this.props.inspection.Buffer;
    var inputbuffer = Buffer("Input Buffer", buffer);

    return (<div className='Inspection'>
      <h4>Inspection</h4>
      <div className='inspection_buffer'>
        { inputbuffer }
      </div>

      <div className='inspection_time'>Current Inspection Time: { this.props.inspection.CurrentInspectionTime }</div>
      
      <div>Current WO: <Workorder workorder={this.props.inspection.CurrentWo} /></div>
    </div>);
  }
}

export default Quality