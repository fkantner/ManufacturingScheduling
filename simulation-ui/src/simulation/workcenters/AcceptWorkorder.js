import React from 'react'
import './AcceptWorkorder.css';

function AcceptWorkorder(props) {
  const name = props.name;
  const title = props.title;
  const body = props.body;

  return <li className={name + ' acceptWorkorder'}>
    <div className={name + '_header wc_header'} >
      <h4>{title}</h4>
    </div>
    <div className={name + '_body wc_body'}>
      { body }
    </div>
  </li>
}

export default AcceptWorkorder