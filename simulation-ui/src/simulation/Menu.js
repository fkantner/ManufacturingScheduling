import React, { Component } from 'react';
import * as foo from './Functions.js';

const maxTimeNodes = 10080;

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

function GenerateOption(num) {
  const minutesInDay = 24*60;
  const day = (Math.floor(num / minutesInDay) + 3) % 7;
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

function NodeSelectors(props) {
  const node = props.node;
  const hideButtons = props.hideButtons;

  return <div>MENU</div>;
  /*
  return (
    <nav className='menu'>
      <div className='node_selectors'>
        <LastButton 
        node={this.state.node}
        length={10080}
        hide={hideButtons}
        onClick={this.changeNode.bind(this)}
        />

        <select className="dayTimeSelect" value={this.state.node} onChange={this.handleChange} >
        { GenerateOptions(1000) }
        </select>

        <NextButton 
        node={this.state.node}
        length={10080}
        hide={hideButtons}
        onClick={this.changeNode.bind(this)}
        />
      </div>
    </nav>
  );
  */
} 

class Menu extends Component {

  constructor(props) {
    super(props);
    /*
    const options = props.options;
    const currentNode = options[0] ? options[0] : 0;
    const hideFirstButton = currentNode === 0;
    const hideLastButton = currentNode === maxTimeNodes - 1;

    this.state = { 
      node: currentNode, 
      hideFirstButton: hideFirstButton,
      hideLastButton: hideLastButton,
      hideButtons: false
    };

    this.changeNode = this.changeNode.bind(this);
    this.handleChange = this.handleChange.bind(this);
    */
  }

  changeNode(i) {
    this.setState( { hideButtons: true }) ;
    this.setState( { node: i } );
    this.setState( { hideButtons: false }) ;
  }
  
  handleChange(event) {
    this.setState( { node: event.target.selectedIndex } );
  }
  
  render() {
    return <NodeSelectors />;
  }
}

export default Menu;