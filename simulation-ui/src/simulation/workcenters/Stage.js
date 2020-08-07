import { Component } from 'react';
import './Stage.css';
import Buffer from '../resources/Buffer';
import AcceptWorkorder from './AcceptWorkorder';

class Stage extends Component {
  render() {
    const outputBuffer = Buffer("Output Buffer:", this.props.stage.OutputBuffer);
    const queue = Buffer("Queue", this.props.stage.Queue);

    const options = {
      name: 'stage',
      title: this.props.stage.Name,
      body: [
        queue,
        outputBuffer
      ]
    };

    return AcceptWorkorder(options);
  }
}

export default Stage;