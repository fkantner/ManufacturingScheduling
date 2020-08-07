import React, { Component } from 'react';
import './Plant.css';
import Workcenter from '../workcenters/Workcenter';
import Dock from '../workcenters/Dock';
import Mes from './Mes';
import Stage from '../workcenters/Stage';

function Transport(props) {
  if (props.workcenter.Name === props.transport.CurrentLocation){
    return <div className='transport'>
      <div className='transport_header'>T</div>
      <div className='transport_body'>
        <div>WO: {props.transport.CargoNumber}</div>
        <div>Dest: {props.transport.Destination}</div>
        <div>Time: {props.transport.TransportTime}</div>
      </div>
    </div>
  }
  else{
    return <div className='transport hidden'>
      
    </div>
  }
}

class Plant extends Component {

  render() {
    //var outputBuffer = Buffer("Output Buffer:", this.props.wc.OutputBuffer);
    var transport = this.props.plant.InternalTransportation;
    var wc_list = this.props.plant.Workcenters.map((wc, index) => {
      if (wc.Name === "Shipping Dock")
      {
        return (
          <div className="plant_leaf dock" key={"Dock:" + index}>
            <Transport workcenter={wc} transport={transport} />
            <Dock dock={wc} />
          </div>
        )
      }
      else if (wc.Name === "Stage")
      {
        return (
          <div className="plant_leaf stage" key={"Stage:" + index}>
            <Transport workcenter={wc} transport={transport} />
            <Stage stage = {wc} />
          </div>
        ) 
      }
      else {
        return(
          <div className="plant_leaf" key={"WC:" + index}>
            <Transport workcenter={wc} transport={transport} />
            <Workcenter wc={wc} />
          </div>
        )
      }
    })

    return <div className='plant'>
      <div className='plant_header'>
        <h4>Plant: {this.props.plant.Name}</h4>
        <Mes mes={this.props.plant.Mes} />
      </div>
      <ul className='plant_body'>
        { wc_list }
      </ul>
    </div>
  }
}

export default Plant