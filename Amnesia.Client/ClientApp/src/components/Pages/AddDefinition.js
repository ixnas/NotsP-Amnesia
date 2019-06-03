import React, { Component } from 'react';
import { AddComponent } from '../AddComponent';

export class AddDefinition extends Component {
    static displayName = AddDefinition.name;

    render() {
        return (
            <div>
                <h1>Voeg definitie toe</h1>
                <p>Op deze pagina kan je een definitie toevoegen. In deze definitie wordt een gegeven opgeslagen (de invoer die jij hieronder aangeeft).</p>
                <p>Er wordt er een definitie aangemaakt aan de hand van een persoonlijke publieke en privé sleutel.</p>
                <AddComponent />
            </div>
        );
    }
}
