import React, { Component } from 'react'
import Workorder from '../resources/Workorder'
import './Quality.css'

class Quality extends Component {
  render() {
    return (<div className='Inspection'>
      <h4>Inspection</h4>
      <div>Current Inspection Time: { this.props.inspection.CurrentInspectionTime }</div>
      <div>Current WO: <Workorder workorder={this.props.inspection.CurrentWo} /></div>
    </div>);
  }
}

export default Quality