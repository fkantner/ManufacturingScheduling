import React, { Component } from 'react';
import './Dock.css';
import Buffer from '../resources/Buffer';

class Dock extends Component {
  render() {
    var outputBuffer = Buffer("Output Buffer:", this.props.dock.OutputBuffer);

    return <div className='dock'>
      <div className='dock_header'>
        <h4>{this.props.dock.Name}</h4>
      </div>
      <div className='dock_body'>
        { outputBuffer }
      </div>
    </div>
  }
}

export default Dock