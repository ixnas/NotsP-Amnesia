import React, { Component } from 'react';
import { Button, Input } from 'reactstrap';
import { DefinitionController } from '../controllers/DefinitionController';

export class AddComponent extends Component {
    static displayName = AddComponent.name;
    constructor(props) {
        super(props);
        this.state = {
            controller: new DefinitionController()
        }
    }

    AddDefinition(e) {
        e.preventDefault();
        var value = document.getElementById("definitionInput").value;
        
        this.state.controller.SetDataToDefinition(value);
        this.state.controller.SendDefinition()
    }

    render() {
        return (
            <div className="container">
                <div className="row">
                    <div className="col-md-4">
                        <p> Gebruik de knop om data toe te voegen </p>
                    </div>
                    <div className="col-md-4">
                        <Input name="definition" id="definitionInput"></Input>
                    </div>

                    <div className="col-md-4">
                        <Button onClick={(e) => this.AddDefinition(e)}> Voeg data toe </Button>
                    </div>
                </div>
            </div>
        );
    }
}