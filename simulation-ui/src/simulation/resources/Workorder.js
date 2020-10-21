import React, { Component } from 'react'
import Op from './Op'
import './Workorder.css'

function fullDisplay(workorder){
  return <div>
      <h5>Current Op</h5><Op op={workorder.CurrentOp} fullDisplay={true} />
    </div>
}

function emptyDisplay(){
  return <div className='short workorder'><h4>No Workorder</h4></div>
}

class Workorder extends Component {
  constructor(props) {
    super(props);
    var shouldShowAll = props.ShowAll ? true : false;
    this.state = { showAll: shouldShowAll };
  }

  render() {
    const wo = this.props.workorder;
    if(wo === null){
      return emptyDisplay();
    }
    else if (this.state.showAll) {
      var opPart;
      opPart = fullDisplay(wo);
              
      return <div className='workorder'>
        <div>
          <h4>Workorder: {wo.Id}</h4>
          <div><label>C/T</label> <span>{wo.CountCompletedOps}/{wo.CountTotalOps}</span></div>
        </div>
        { opPart }
      </div>
    }
    else {
      return <div className='short workorder'>
        <div>
          <span>WO: {wo.Id}</span> 
          <span> @ <Op op={wo.CurrentOp} fullDisplay={false} /></span> 
          <span> <label>C/T</label> <span>{wo.CountCompletedOps}/{wo.CountTotalOps}</span></span>
        </div>
      </div>
    }
  }
}

export default Workorder