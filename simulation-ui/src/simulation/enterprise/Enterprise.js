import React, { Component } from 'react';
import Plant from '../plants/Plant';
import './Enterprise.css';

class Enterprise extends Component {
  render () {
    var plants = this.props.enterprise.Plants;
    var index = this.props.index;
    return(
      <div key="enterprise" className="enterprise">
        <h1>Test Enterprise Simulation</h1>
        {plants.map((plant, i) => {
          return (
            <div key={"Plant" + index + ":" + i}>
              <Plant plant={plant} />
            </div>
          )
        })}
      </div>
    );
  }
}

export default Enterprise;
