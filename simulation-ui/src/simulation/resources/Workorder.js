import React, { Component } from 'react'
import Op from './Op'
import './Workorder.css'

function fullDisplay(workorder){
  return <div><h5>Current Op</h5><Op op={workorder.CurrentOp} /></div>
}

function partialDisplay(workorder){
  return <div>
      <h5>Current Op: {workorder.CurrentOpType}</h5>
    </div>
}

function Display(workorder, showAll) {
  var opPart;
  if (showAll) { opPart = fullDisplay(workorder); }
  else { opPart = partialDisplay(workorder); }

  var classname = showAll ? 'workorder' : 'short workorder';
    
  return <div className = {classname}>
    <h4>Workorder: {workorder.Id}</h4>
    { opPart }
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
    } else {
      return Display(wo, this.state.showAll);
    }
  }
}

export default Workorder