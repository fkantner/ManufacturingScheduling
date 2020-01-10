import React, { Component } from 'react'
import './Op.css'

class Op extends Component {
  render() {
    return <div className='op'>
      <div className="OpPart"><h4>Op</h4></div>
      <div className="OpPart">
        <h5>Type: {this.props.op.Type}</h5>
        <p>Time to Complete: {this.props.op.EstTimeToComplete}</p>
        <p>Setup Time: {this.props.op.SetupTime}</p>
      </div>
    </div>
  }
}

export default Op