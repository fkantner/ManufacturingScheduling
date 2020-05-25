import React, { Component } from 'react';
import SimulationData from '../data/test.json';
import Day from './Day';
import Enterprise from './enterprise/Enterprise';
import './Reset.css';
import './Simulation.css';

function LastButton(props) {
  const notAtFront = props.node > 0;
  if (notAtFront){
    return (<a href="#" key={"last"} className="lastButton" onClick={props.onClick.bind(this, props.node - 1)}>{"<="}</a>);
  }
  else { return <a href="#" className="disabled hidden" >{"<="}</a> }
}

function NextButton(props) {
  const notAtEnd = props.node < props.length - 1;
  if (notAtEnd) {
    return (<a href="#" key={"next"} className="nextButton" onClick={props.onClick.bind(this, props.node + 1)}>{"=>"}</a>);
  }
  else { return <a href="#" className="disabled hidden" >{"=>"}</a> }
}

function ParseDay(day){
  var dayOfWeek;
  switch(day) {
    case 0: dayOfWeek = 'Su'; break;
    case 1: dayOfWeek = 'Mo'; break;
    case 2: dayOfWeek = 'Tu'; break;
    case 3: dayOfWeek = 'We'; break;
    case 4: dayOfWeek = 'Th'; break;
    case 5: dayOfWeek = 'Fr'; break;
    case 6: dayOfWeek = 'Sa'; break;
    default: dayOfWeek = 'Er';
  }
  return dayOfWeek;
}

function ParseTime(time) {
  var hour = FrontLoadZeros(Math.floor(time / 60));
  var minute = FrontLoadZeros(time % 60);

  return hour + ":" + minute;
}

function FrontLoadZeros(number) {
  var answer;
  if (number === 0){ answer = "00"; }
  else if (number < 10) { answer = "0" + number; }
  else { answer = number.toString(); }
  return answer;
}

class Simulation extends Component {
  constructor(props) {
    super(props);
    this.state = { node: 0 };
    this.changeNode = this.changeNode.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }

  changeNode(i) {
    this.setState( { node: i } );
  }

  handleChange(event) {
    this.setState( { node: event.target.selectedIndex } );
  }

  render () {
    var index = this.state.node;
    var simulationDetail = SimulationData[index];
    
    var daytime = simulationDetail.DayTime;
    
    return (
      <div>
        <h1>Simulation UI</h1>
        
        <div className='node_selectors'>
          <LastButton 
            node={this.state.node}
            length={SimulationData.length}
            onClick={this.changeNode.bind(this)}
          />

          <select className="dayTimeSelect" value={this.state.node} onChange={this.handleChange} >
            {SimulationData.map((data, index) => {
              return <option key={"select:"+ index} value={index}>{ParseDay(data.DayTime.Day) + "--" + ParseTime(data.DayTime.Time)}</option>
            })}
          </select>

          <NextButton 
            node={this.state.node}
            length={SimulationData.length}
            onClick={this.changeNode.bind(this)}
          />
        </div>  

        <div className="simulation_node">
          <div key={"Day" + index}>
            <Day day={ParseDay(daytime.Day)} time={ParseTime(daytime.Time)} />
          </div>

          <Enterprise enterprise={simulationDetail.Enterprise} index = {index} />
          
        </div>
  
      </div>
      
    );
  }
}

export default Simulation