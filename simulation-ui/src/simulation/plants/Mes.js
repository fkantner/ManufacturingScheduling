import React, { Component } from 'react';
import './Mes.css';

class Mes extends Component {
  render() {
    var name = this.props.mes.Name;
    var inventories = this.props.mes.LocationInventories;
    var wc_list = Object.entries(inventories).map(([key, value]) => {
      var list;
      if(value.count === 0) { list = <li>[]</li> }
      else {
        list = value.map((obj, index) => {
          var virtKey = "Virt" + key + index;
          return <li key={virtKey}>{obj.Id}</li>;
        });
      }


      return <ul className="mesWcList" key={key}>{key}: {list}</ul>
    });
    

    return <div className='mes'>
      <h4>MES: {name}</h4>
      <div className='virtualWorkcenters'>WC: {wc_list}</div>
    </div>
  }
}

export default Mes;