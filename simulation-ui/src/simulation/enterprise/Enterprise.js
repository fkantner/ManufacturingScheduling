import React, { Component } from 'react';
import Plant from '../plants/Plant';
import Erp from './Erp';
import './Enterprise.css';

function Transport(props) {
  const location = props.transport.CurrentLocation;
  const cargo = props.transport.CurrentCargo;

  const html = function(attached){
    return <div className='externaltransport'>
    <div className='transport_header'>External Transportation {attached}</div>
      <div className='transport_body'>
        {cargo}
      </div>
    </div>
  }
  
  if (location !== null) { return html(location); }
  return html("Unattached");
}

class Enterprise extends Component {
  render () {
    var plants = this.props.enterprise.Plants;
    var index = this.props.index;
    var transport = this.props.enterprise.Transport;
    return(
      <div key="enterprise" className="enterprise">
        <h1>Test Enterprise Simulation</h1>
        <div className='enterprise_overview'>
          <Erp erp={this.props.enterprise.Erp} />
          <Transport transport={transport} />
        </div>
        <div className='enterprise_plants'>
          {plants.map((plant, i) => {
            return (
              <div key={"Plant" + index + ":" + i} className="PlantDiv">
                <Plant plant={plant} />
              </div>
            )
          })}
        </div>
      </div>
    );
  }
}

export default Enterprise;
