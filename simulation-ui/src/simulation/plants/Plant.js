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
  BuildLeaf(options) {
    const cName = "plant_leaf " + options.type;
    const key = options.type + ":" + options.index;
    return (
      <div className={cName} key={key}>
        <Transport workcenter={options.wc} transport={options.transport} />
        {options.innerObj}
      </div>
    )
  }

  render() {
    //var outputBuffer = Buffer("Output Buffer:", this.props.wc.OutputBuffer);
    const transport = this.props.plant.InternalTransportation;
    const shippingList = this.props.plant.Workcenters
        .filter((x) => x.Name === "Shipping Dock" || x.Name === "Stage")
        .map((wc, index) => {
        if (wc.Name === "Shipping Dock")
        {
          return this.BuildLeaf({
            type: 'dock',
            index: index,
            wc: wc,
            transport: transport,
            innerObj: ( <Dock dock={wc}/> )
          });
        }
        else if (wc.Name === "Stage")
        {
          return this.BuildLeaf({
            type: 'stage',
            index: index,
            wc: wc,
            transport: transport,
            innerObj: ( <Stage stage={wc}/>)
          });
        }
        return '';
      });

    const wc_list = this.props.plant.Workcenters
      .filter((x) => x.Name !== "Shipping Dock" && x.Name !== "Stage")
      .map((wc, index) => {
        return this.BuildLeaf({
          type: 'wc',
          index: index,
          wc: wc,
          transport: transport,
          innerObj: (<Workcenter wc={wc}/>)
        });
      });

    const shipping = (
      <div className="plant_branch shipping">
        {shippingList}
      </div>
    );

    return <div className='plant'>
      <div className='plant_header'>
        <h4>Plant: {this.props.plant.Name}</h4>
        <Mes mes={this.props.plant.Mes} />
      </div>
      <ul className='plant_body'>
        { shipping }
        { wc_list }
      </ul>
    </div>
  }
}

export default Plant