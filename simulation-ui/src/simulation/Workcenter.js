import React, { Component } from 'react'

class Workcenter extends Component {
  render() {
    return <div>
      <h4>Name: {this.props.wc.Name}</h4>
      <p>Machine ...</p>
      <p>OutputBuffer ...</p>
      <p>Inspection ...</p>
    </div>
  }
}

export default Workcenter