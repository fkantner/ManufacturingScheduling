import React, { Component } from 'react'

class Day extends Component {
  render() {
    return <h3>
      <b>Day: { this.props.day }</b> Time: { this.props.time }
    </h3>
  }
}

export default Day