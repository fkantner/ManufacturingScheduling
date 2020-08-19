import React, { Component } from 'react';
//import logo from './logo.svg';
import Simulation from './simulation/Simulation';
import Menu from './simulation/Menu';
import './App.css';
import { Switch, Route } from 'react-router-dom';
import { useParams } from "react-router";

class App extends Component {
  render () {
    
    return (
      // props = { node: this.state.node, hideButtons: this.state.hideButtons };

      <div className="App">
        <Menu />

        <Switch>
          <Route exact path="/" component={MainSimulation}/>
          <Route path="/:id" component={MainSimulation}/>
        </Switch>
      </div>
    );
  }
}

function GetFilename(test, timeNode) {
  return './src/data/default1.json';
}

function LoadFile(test, timeNode) {
  var loaded = false;
  //const filename = `./data/${test}${timeNode}.json`;
  const filename = `./data/default1.json`;
  const fileref = document.createElement('script');
  fileref.type = "application/javascript";
  fileref.src = filename;

  fileref.onload = function(){
    loaded = true;
  };

  document.body.appendChild(fileref);

  while(!loaded) {
    // loop;
  }
  return fileref.innerHTML();
}

const getFile = (page) => {
  const filename = './data/default1.json';
  const details = import(filename);
  return details;
}

export const MainSimulation = () => {
  let { index } = useParams();
  //const filename = GetFilename('default', 0);
  //const filename = "./data/default1.json";
  //const simulationDetail = require('./data/default1.json');
  //const simulationDetail = import(/* webpackMode: "eager" */ GetFilename('default', 0));
  const simulationDetail = LoadFile('default', 0);
  //const simulationDetail = require(`${filename}`);
  //const simulationDetail = require('./data/default' + index + '.json');
  return <Simulation node={index} options={simulationDetail} />;
};

export default App;
