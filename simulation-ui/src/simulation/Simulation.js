import React, { Component } from 'react'
import SimulationData from '../data/test.json'
import Day from './Day'
import Workcenter from './workcenters/Workcenter'
import './Simulation.css'

class Simulation extends Component {
  constructor(props) {
    super(props);
    this.state = { node: 0 };
  }

  changeNode(i) {
    this.setState( { node: i } );
  }

  render () {
    var index = this.state.node;
    var simulationDetail = SimulationData[index];
    
    var daytime = simulationDetail.DayTime;
    var wc = simulationDetail.Workcenter;

    return (
      <div>
        <h1>Simulation UI</h1>
        
        <div class='node_selectors'>
          {SimulationData.map((data, index) => {
            return (
              <a href="#" key={index} onClick={this.changeNode.bind(this, index)}>{index}</a>
            );
          })}
          
        </div>  

        <div className="simulation_node">
          <div key={"Day" + index}>
            <Day day={daytime.Day} time={daytime.Time} />
          </div>

          <div key={"WC" + index }>
            <Workcenter wc={wc} />
          </div>
        </div>
  
      </div>
      
    );
  }
}

export default Simulation