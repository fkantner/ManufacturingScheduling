import React, { Component } from 'react'
import * as foo from '../Functions'
import './Op.css'

class Op extends Component {

  render() {
    if (this.props.fullDisplay !== true) {
      return <span>{foo.GetOpType(this.props.op.Type)}</span>
    }
    else {
      return <div className='op'>
      <div className="OpPart"><h4>Op</h4></div>
      <div className="OpPart">
        <h5>Type: {foo.GetOpType(this.props.op.Type)}</h5>
        <p>Time to Complete: {this.props.op.EstTimeToComplete}</p>
        <p>Setup Time: {this.props.op.SetupTime}</p>
      </div>
    </div>
    }
  }
}

export default Op