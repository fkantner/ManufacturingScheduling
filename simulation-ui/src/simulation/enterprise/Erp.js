import React, { Component } from 'react';
import './Erp.css';

class Erp extends Component {
    
    render() {
        const inventories = this.props.erp.LocationInventories;
        const createWoList = function(key){
            return function(values) {
                if(values.count ===0){ return <li>[]</li>; }
                
                return values.map((obj, index) => {
                    const domKey = "Virt" + key + index;
                    return <li key={domKey}>{obj.Id}</li>;
                });
            };
        };
    
        const createPlantList = function(name){
            return function(wos) {
                const woList = createWoList(name);
                const list = woList(wos);
    
                return <ul className="erpPlantList" key={name}>{name}: {list}</ul>;
            }
        };

        const plant_list = Object.entries(inventories).map(([plantName, wos]) => {
            const plantList = createPlantList(plantName);
            return plantList(wos);
        });
        
        return <div className='erp'>
            <h3>ERP: </h3>
            <div className='virtualPlants'>{plant_list}</div>
        </div>
    }
}

export default Erp;