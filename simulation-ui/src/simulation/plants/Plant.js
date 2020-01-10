import React, { Component } from 'react';
import './Plant.css';
import Workcenter from '../workcenters/Workcenter';
//import Buffer from '../resources/Buffer'

class Plant extends Component {
  render() {
    //var outputBuffer = Buffer("Output Buffer:", this.props.wc.OutputBuffer);

    var wc_list = this.props.plant.Workcenters.map((wc, index) => {
      return(
        <div key={"WC:" + index}>
          <Workcenter wc={wc} />
        </div>
      )
    })

    return <div className='plant'>
      <div className='plant_header'>
        <h4>Plant: {this.props.plant.Name}</h4>
      </div>
      <div className='plant_body'>
        { wc_list }
      </div>
    </div>
  }
}

export default Plant