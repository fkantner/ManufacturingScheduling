import React, { Component } from 'react';
import './Stage.css';
import Buffer from '../resources/Buffer';

class Stage extends Component {
  render() {
    var outputBuffer = Buffer("Output Buffer:", this.props.stage.OutputBuffer);
    var queue = Buffer("Queue", this.props.stage.Queue);

    return <div className='stage'>
      <div className='stage_header'>
        <h4>{this.props.stage.Name}</h4>
      </div>
      <div className='stage_body'>
        { queue }
        { outputBuffer }
      </div>
    </div>
  }
}

export default Stage;