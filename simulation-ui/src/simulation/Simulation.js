import React, { Component } from 'react';
import Day from './Day';
import Enterprise from './enterprise/Enterprise';
import './Reset.css';
import './Simulation.css';
import * as foo from './Functions.js';

class Simulation extends Component {
  render () {
    //debugger;
    const index = this.props.node;
    const details = this.props.options;
    const daytime = details.DayTime;
        
    return (
      <div>
        <h1>Simulation UI</h1>
        
        <div className="simulation_node">
          <div key={"Day" + index}>
            <Day day={foo.ParseDay(daytime.Day)} time={foo.ParseTime(daytime.Time)} />
          </div>

          <Enterprise enterprise={details.Enterprise} index={index} />
          
        </div>
      </div>
    );
  }
}

export default Simulation