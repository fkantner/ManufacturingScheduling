import React, { Component } from 'react';
import * as foo from './Functions.js';

function LastButton(props) {
  const notAtFront = props.node > 0;
  const hidden = props.hide;
  if (notAtFront || hidden){
    return (<button key={"last"} className="lastButton" onClick={() => props.onClick(props.node - 1)}>{"<="}</button>);
  }
  else { return <button className="disabled hidden" >{"<="}</button> }
}

function NextButton(props) {
  const notAtEnd = props.node < props.length - 1;
  const hidden = props.hide;
  if (notAtEnd || hidden) {
    return (<button key={"next"} className="nextButton" onClick={() => props.onClick(props.node + 1)}>{"=>"}</button>);
  }
  else { return <button className="disabled hidden" >{"=>"}</button> }
}

function GenerateOption(num) {
  const minutesInDay = 24*60;
  const day = (Math.floor(num / minutesInDay)) % 7;
  const minute = num % minutesInDay;
  return <option key={"select:" + num} value={num}>{foo.ParseDay(day) + "--" + foo.ParseTime(minute)}</option>
}

function GenerateOptions(max) {
  let arry = [];
  for(let i=0; i<max; i++){
    arry.push(GenerateOption(i));
  }
  return arry;
}

function GenerateTestOptions(list) {
  let arry = [];
  list.forEach((x) => {
    arry.push(<option key={"selectTest:" + x} value={x}>{x}</option>);
  });
  return arry;
}

class Menu extends Component {

  constructor(props) {
    super(props);
    this.handleDataChange = this.props.handleDataChange;
    this.state = {
      index: this.props.index,
      length: 0,
      hideButtons: false,
      error: null,
      test: 'default',
      list: ['default']
    };
    
    this.getCount();
    this.getTests();
    this.changeNode = this.changeNode.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }

  getCount() {
    fetch("http://localhost:3003/count/" + this.state.test)
    .then(res => { return res.json(); })
    .then(data => {
      this.setState({
        length: data
      });
    })
    .catch((error) => {
      this.setState({
        length: 0,
        error
      });
    });
  }

  getTests() {
    fetch("http://localhost:3003/types")
    .then(res => { return res.json(); })
    .then(data => {
      this.setState({
        list: data
      });
    })
    .catch((error) => {
      this.setState({
        length: 0,
        error
      });
    });
  }

  changeNode(i) {
    this.setState( { hideButtons: true }) ;
    this.handleDataChange(this.state.test, i);
    this.setState( { index: i, hideButtons: false }) ;
  }
 
  handleChange(event) {
    const newI = event.target.selectedIndex;
    this.handleDataChange(this.state.test, newI);
    this.setState( { index: newI } );
  }

  handleTestChange(event) {
    const test = event.target.selectedTest;
    this.handleDataChange(test, this.state.index);
    this.setState( { test: test } );
  }

  render() {
    return (
      <nav className='menu'>
        <div className='test_selectors'>
          <select className="testSelect" value={this.state.test} onChange={this.handleTestChange} >
            { GenerateTestOptions(this.state.list) }
          </select>
        </div>

        <div className='node_selectors'>
          
          <LastButton 
          node={this.state.index}
          length={this.state.length}
          hide={this.state.hideButtons}
          onClick={this.changeNode}
          />
          
          <select className="dayTimeSelect" value={this.state.index} onChange={this.handleChange} >
            { GenerateOptions(this.state.length) }
          </select>

          <NextButton 
          node={this.state.index}
          length={this.state.length}
          hide={this.state.hideButtons}
          onClick={this.changeNode}
          />

        </div>
      </nav>
    );
  }
}

export default Menu;