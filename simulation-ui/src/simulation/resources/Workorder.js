import React, { Component } from 'react'
import Op from './Op'
import './Workorder.css'

function fullDisplay(workorder){
  return <div className='workorder'>
  <h4>Workorder: {workorder.Id}</h4>
  <p>Current Op: <Op op={workorder.CurrentOp} /></p>
</div>
}

function emptyDisplay(){
  return <div className='workorder'><h4>No Workorder</h4></div>
}

class Workorder extends Component {

  render() {
    const wo = this.props.workorder;
    if(wo === null){
      return emptyDisplay();
    } else {
      return fullDisplay(wo);
    }
  }
}

export default Workorder