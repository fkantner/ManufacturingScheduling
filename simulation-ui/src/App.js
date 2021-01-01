import React, { Component } from 'react';
import Simulation from './simulation/Simulation';
import Menu from './simulation/Menu';
import './App.css';

class App extends Component {
  constructor(props){
    super(props);
    this.state = {
      loaded: false,
      index: 0,
      error: null,
      items: {},
      test: 'Default'
    };
    this.getData = this.getData.bind(this);
    this.reset = this.reset.bind(this);
  }

  reset() {
    this.setState({
      loaded: false,
      index: 0,
      error: null,
      items: {},
      test: 'Default'
    });
  }

  getData(test, index) {
    this.reset();

    fetch("http://localhost:3003/file/" + test + '_' + index)
    .then(res => {return res.json();})
    .then(data => {
      this.setState({
        index: index,
        loaded: true,
        items: data,
        test: test
      });
    })
    .catch((error) => {
      this.setState({
        loaded: true,
        error
      });
    });
  }
  
  componentDidMount() {
    this.getData("Default", 0);
  }

  render () {
    const { index, loaded, error, items } = this.state;

    if(error) {
      return <div>Error: { error.message }</div>
    }
    
    if (!loaded){
      return <div>Loading...</div>
    } 
    
    return (
      <div className="App">
        <Menu handleDataChange={this.getData} index={this.state.index} test={this.state.test} />
        <Simulation node={index} options={items} />
      </div>
    );
  }
}

export default App;