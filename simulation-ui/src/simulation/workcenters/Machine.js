import React, { Component } from 'react'
import Workorder from '../resources/Workorder'
import './Machine.css'

class Machine extends Component {
  render() {
    return <div className='machine'>
      <h4>Name: {this.props.machine.Name}</h4>
      <p>Setup Time: {this.props.machine.SetupTime}</p>
      <p>LastType: {this.props.machine.LastType}</p>
      <p>Est Time to Complete: {this.props.machine.EstTimeToComplete}</p>
      <p>CurrentWorkorder: </p><Workorder workorder={this.props.machine.CurrentWorkorder} />
      <p>Input Buffer ...</p>
    </div>
  }
}

export default Machine