import React, { Component } from 'react'
import SimulationData from '../data/test.json'
import Day from './Day'
import Workcenter from './workcenters/Workcenter'

class Simulation extends Component {
  render () {
    return (
      <div>
        <h1>Simulation UI</h1>
        {SimulationData.map((simulationDetail, index)=>{
          const day = simulationDetail.Day;
          if (day)
          {
            return <div key={"Day" + index}>
              <Day day={simulationDetail.Day} time={simulationDetail.Time} />
              </div>
          }
          else
          {
            return <div key={"WC" + index }>
              <Workcenter wc={simulationDetail} />
            </div>
          }
        })}
      </div>
    )
  }
}

export default Simulation