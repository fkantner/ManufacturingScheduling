import React, { Component } from 'react';
import SimulationData from '../data/test.json';
import Day from './Day';
import Workcenter from './workcenters/Workcenter';
import './Simulation.css';

function LastButton(props) {
  const notAtFront = props.node > 0;
  if (notAtFront){
    return (<a href="#" key={"last"} onclick={props.onClick.bind(this, props.node - 1)}>{"<="}</a>);
  }
  else { return ''; }
}

function NextButton(props) {
  const notAtEnd = props.node < props.length - 1;
  if (notAtEnd) {
    return (<a href="#" key={"next"} onClick={props.onClick.bind(this, props.node + 1)}>{"=>"}</a>);
  }
  else { return ''; }
}

class Simulation extends Component {
  constructor(props) {
    super(props);
    this.state = { node: 0 };
    this.changeNode = this.changeNode.bind(this);
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
        
        <div className='node_selectors'>
          <LastButton 
            node={this.state.node}
            length={SimulationData.length}
            onClick={this.changeNode.bind(this)}
          />

          {SimulationData.map((data, index) => {
            return (
              <a href="#" key={index} onClick={this.changeNode.bind(this, index)}>{index}</a>
            );
          })}

          <NextButton 
            node={this.state.node}
            length={SimulationData.length}
            onClick={this.changeNode.bind(this)}
          />
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