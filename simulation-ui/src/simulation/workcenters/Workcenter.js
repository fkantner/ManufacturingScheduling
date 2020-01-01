import React, { Component } from 'react'
import './Workcenter.css'
import Machine from './Machine'

class Workcenter extends Component {
  render() {
    return <div className='workcenter'>
      <h4>Name: {this.props.wc.Name}</h4>
      <p>OutputBuffer ...</p>
      <p>Inspection ...</p>
      <p>Machine: </p><Machine machine={this.props.wc.Machine} />
    </div>
  }
}

export default Workcenter