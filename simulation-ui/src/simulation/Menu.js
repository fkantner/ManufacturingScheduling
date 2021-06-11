import React, { Component } from 'react';

function OptionPaginationButton(props) {
  const hide = props.hide;
  const hideBecauseOfIndex = props.hideForIndex;
  if (hide || hideBecauseOfIndex)
  {
    return (<button key={props.buttonName} className={props.buttonName + "Button"} onClick={() => props.onClick(props.newIndex)}>{props.symbol}</button>);
  }
  else
  {
    return <button className="disabled hidden">{props.symbol}</button>
  }
}

function OptionTestButton(props) {
  return (<button key={props.testName} className={props.testName + "Button"} onClick={() => props.onClick(props.testName)}>{props.testName}</button>);
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
      test: this.props.test,
      list: ['Default']
    };
    
    this.getCount();
    this.getTests();
    this.changeNode = this.changeNode.bind(this);
    this.changeTest = this.changeTest.bind(this);
    this.handleChange = this.handleChange.bind(this);
    this.handleTestChange = this.handleTestChange.bind(this);
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

  changeTest(i) {
    this.handleDataChange(i, this.state.index);
    this.setState({ test: i });
  }
 
  handleChange(event) {
    const newI = event.target.selectedIndex;
    this.handleDataChange(this.state.test, newI);
    this.setState( { index: newI } );
  }

  handleTestChange(event) {
    const test = event.target.value;
    this.handleDataChange(test, this.state.index);
    this.setState( { test: test } );
  }

  render() {
    const day = 60*24;

    const testList = this.state.list.sort().map((test) => {
      if(test === this.state.test){
        return <span key={test} className="disabled">{test}</span>
      }
      return (
        <OptionTestButton
          key={test}
          testName={test}
          onClick={this.changeTest}
        />
      );
    });

    return (
      <nav className='menu'>
        <div className='test_selectors'>
          {testList}
          
        </div>

        <div className='node_selectors'>
          <OptionPaginationButton
            hide={this.state.hideButtons}
            hideForIndex={this.state.index > 0}
            buttonName={"First"}
            newIndex={0}
            onClick={this.changeNode}
            symbol={"First"}
          />

          <OptionPaginationButton
            hide={this.state.hideButtons}
            hideForIndex={this.state.index > day}
            buttonName={"lastDay"}
            newIndex={this.state.index - day}
            onClick={this.changeNode}
            symbol={"-Day"}
          />

          <OptionPaginationButton
            hide={this.state.hideButtons}
            hideForIndex={this.state.index > 60}
            buttonName={"lastHour"}
            newIndex={this.state.index - 60}
            onClick={this.changeNode}
            symbol={"-60"}
          />

          <OptionPaginationButton
            hide={this.state.hideButtons}
            hideForIndex={this.state.index > 9}
            buttonName={"last10"}
            newIndex={this.state.index - 10}
            onClick={this.changeNode}
            symbol={"-10"}
          />

          <OptionPaginationButton
            hide={this.state.hideButtons}
            hideForIndex={this.state.index > 0}
            buttonName={"last"}
            newIndex={this.state.index - 1}
            onClick={this.changeNode}
            symbol={"<="}
          />
                    
          <OptionPaginationButton
            hide={this.state.hideButtons}
            hideForIndex={this.state.index < this.state.length - 1}
            buttonName={"next"}
            newIndex={this.state.index + 1}
            onClick={this.changeNode}
            symbol={"=>"}
          />

          <OptionPaginationButton
            hide={this.state.hideButtons}
            hideForIndex={this.state.index < this.state.length - 11}
            buttonName={"next10"}
            newIndex={this.state.index + 10}
            onClick={this.changeNode}
            symbol={"+10"}
          />

          <OptionPaginationButton
            hide={this.state.hideButtons}
            hideForIndex={this.state.index < this.state.length - 61}
            buttonName={"nextHour"}
            newIndex={this.state.index + 60}
            onClick={this.changeNode}
            symbol={"+60"}
          />

          <OptionPaginationButton
            hide={this.state.hideButtons}
            hideForIndex={this.state.index < this.state.length - (day + 1)}
            buttonName={"nextDay"}
            newIndex={this.state.index + day}
            onClick={this.changeNode}
            symbol={"+Day"}
          />
          
          <OptionPaginationButton
            hide={this.state.hideButtons}
            hideForIndex={this.state.index < this.state.length - 1}
            buttonName={"Last"}
            newIndex={this.state.length - 1}
            onClick={this.changeNode}
            symbol={"Last"}
          />

        </div>
      </nav>
    );
  }
}

export default Menu;