import React, { Component } from 'react'
import Workorder from '../resources/Workorder'
import './Machine.css'
import Buffer from '../resources/Buffer'
import * as foo from '../Functions'

class Machine extends Component {
  render() {
    const buffer = this.props.machine.InputBuffer;
    var inputbuffer = Buffer("Input Buffer", buffer);

    return (<div className='machine' key={"Machine" + this.props.machine.Name}>
      <div className='machine_header'>
        <h4>Machine Type: {this.props.machine.Name}</h4>
      </div>
      <div className='machine_body'>

        { inputbuffer }

        <div className='machine_data'>
          
          <h4>Operation</h4>
          <table className='machine_time'>
            <tbody>
              <tr>
                <th>S</th>
                <th>C</th>
                <th className='machine_last_type'>Last</th>
              </tr>
              <tr>
                <td>{this.props.machine.SetupTime}</td>
                <td>{this.props.machine.EstTimeToComplete}</td>
                <td>{foo.GetOpType(this.props.machine.LastType)}</td>
              </tr>              
            </tbody>
          </table>
          
          <div className='machine_current_wo'>
            <h4>Current Workorder</h4>
            <Workorder workorder={this.props.machine.CurrentWorkorder} ShowAll={true} />
          </div>

        </div>
        

      </div>
      
    </div>);
  }
}

export default Machine