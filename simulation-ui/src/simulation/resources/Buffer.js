import React from 'react';
import Workorder from '../resources/Workorder'

function Buffer (buffer){
  if (buffer.length === 0) { return <div>Empty Buffer</div> }
  else {
    var inputbuffer = buffer.map((wo, index) => 
      {
        if (wo === undefined || wo.Id === undefined){ return null; }
        else {
          return <li key={"WO"+index}><Workorder workorder={wo} /></li>
        }
      }
    )
    return <ul>
      {inputbuffer}
    </ul>
  }
}

export default Buffer