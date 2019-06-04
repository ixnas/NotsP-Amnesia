import React, { Component } from 'react';
import { Button, Input } from 'reactstrap';
import { DefinitionController } from '../controllers/DefinitionController';

export class DeleteComponent extends Component {
    static displayName = DeleteComponent.name;
    constructor(props) {
        super(props);
        this.state = {
            controller: new DefinitionController()
        }
    }

    AddMutation(e) {
        e.preventDefault();
        var value = document.getElementById("mutationInput").value;
        
        this.state.controller.SetDataToDefinition(value, true);
    }

    render() {
        return (
            <div className="container">
                <div className="row">
                    <div className="col-md-6">
                        <Input name="mutation" id="mutationInput" placeholder="Definitie Hash"></Input>
                    </div>

                    <div className="col-md-6">
                        <Button onClick={(e) => this.AddMutation(e)}>Verwijder Definitie</Button>
                    </div>
                </div>
            </div>
        );
    }
}