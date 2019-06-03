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
    }

    render() {
        return (
            <div className="container">
                <div className="row">
                    <div className="col-md-6">
                        <Input name="definition" id="definitionInput" placeholder="Persoonsgegeven"></Input>
                    </div>

                    <div className="col-md-6">
                        <Button onClick={(e) => this.AddDefinition(e)}>Voeg definitie toe</Button>
                    </div>
                </div>
            </div>
        );
    }
}
