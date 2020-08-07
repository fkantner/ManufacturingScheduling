import React, { Component } from 'react';
import Day from './Day';
import Enterprise from './enterprise/Enterprise';
import './Reset.css';
import './Simulation.css';

function LastButton(props) {
  const notAtFront = props.node > 0;
  const hidden = props.hide;
  if (notAtFront || hidden){
    return (<a href="#" key={"last"} className="lastButton" onClick={props.onClick.bind(this, props.node - 1)}>{"<="}</a>);
  }
  else { return <a href="#" className="disabled hidden" >{"<="}</a> }
}

function NextButton(props) {
  const notAtEnd = props.node < props.length - 1;
  const hidden = props.hide;
  if (notAtEnd || hidden) {
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
  if (number === 0){ return "00"; }
  if (number < 10) { return "0" + number; }
  return number.toString();
}

function GenerateOption(num) {
  const minutesInDay = 24*60;
  const day = (Math.floor(num / minutesInDay) + 3) % 7;
  const minute = num % minutesInDay;
  return <option key={"select:" + num} value={num}>{ParseDay(day) + "--" + ParseTime(minute)}</option>
}

function CombineOptions(opts, newopt){
  return opts.concat(newopt());
}

function GenerateOptions(max) {
  let arry = [];
  for(let i=0; i<max; i++){
    arry.push(GenerateOption(i));
  }
  return arry;
}

class Simulation extends Component {
  constructor(props) {
    super(props);
    this.state = { node: 0, hideButtons: false};
    this.changeNode = this.changeNode.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }

  changeNode(i) {
    this.setState( { hideButtons: true }) ;
    this.setState( { node: i } );
    this.setState( { hideButtons: false }) ;
  }

  handleChange(event) {
    this.setState( { node: event.target.selectedIndex } );
  }

  render () {
    const index = this.state.node;
    const simulationDetail = require('../data/default' + index + '.json');
    const daytime = simulationDetail.DayTime;
    const hideButtons = this.state.hideButtons;
    
    return (
      <div>
        <h1>Simulation UI</h1>
        
        <div className='node_selectors'>
          <LastButton 
            node={this.state.node}
            length={200}
            hide={hideButtons}
            onClick={this.changeNode.bind(this)}
          />

          <select className="dayTimeSelect" value={this.state.node} onChange={this.handleChange} >
            { GenerateOptions(1000) }
          </select>

          <NextButton 
            node={this.state.node}
            length={200}
            hide={hideButtons}
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