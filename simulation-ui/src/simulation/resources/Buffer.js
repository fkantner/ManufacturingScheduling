import React from 'react';
import Workorder from '../resources/Workorder'

function Buffer (title, buffer){
  if (buffer.length === 0) { return <div className="buffer"><h5>Empty {title}</h5></div> }
   
  var inputbuffer = buffer.map((wo, index) =>
    {
      if (wo === undefined || wo.Id === undefined){ return null; }
      else {
        return <li key={"WO"+index}><Workorder workorder={wo} /></li>
      }
    }
  )
  return( 
    <div className="buffer">
      <h5>{title}</h5>
      <ul>
        {inputbuffer}
      </ul>
    </div>
  );
}

export default Buffer