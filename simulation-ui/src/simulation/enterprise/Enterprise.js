import React, { Component } from 'react';
import Plant from '../plants/Plant';
import './Enterprise.css';

function Transport(props) {
  const location = props.transport.CurrentLocation;
  const cargo = props.transport.CurrentCargo;
  const plant = props.plant;

  const html = function(attached){
    return <div className='externaltransport'>
    <div className='transport_header'>External Transportation {attached}</div>
      <div className='transport_body'>
        {cargo}
      </div>
    </div>
  }
  
  if (plant === null) { //Showing in transit
    if (location === null)
    {
      return html("Unattached");
    }

    return '';
  }

  if (location !== null && plant.Name === location){
    return html("");
  }
  
  return ''
}

class Enterprise extends Component {
  render () {
    var plants = this.props.enterprise.Plants;
    var index = this.props.index;
    var transport = this.props.enterprise.Transport;
    return(
      <div key="enterprise" className="enterprise">
        <h1>Test Enterprise Simulation</h1>
        <Transport plant={null} transport={transport} />
        {plants.map((plant, i) => {
          return (
            <div key={"Plant" + index + ":" + i} className="PlantDiv">
              <Transport plant={plant} transport={transport} />
              <Plant plant={plant} />
            </div>
          )
        })}
      </div>
    );
  }
}

export default Enterprise;
