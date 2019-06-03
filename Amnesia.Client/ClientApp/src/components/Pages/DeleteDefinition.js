import React, { Component } from 'react';
import { DeleteComponent } from '../DeleteComponent';

export class DeleteDefinition extends Component {
    static displayName = DeleteDefinition.name;

    render() {
        return (
            <div>
                <h1>Verwijder een definitie</h1>
                <p>Op deze pagina kan je een definitie verwijderen. Dit doe je aan de hand van een definitie hash, door deze in te voeren en op de knop te drukken wordt het doel van het onderzoek vastgelegd waarin de definitie die aangegeven is te verwijderen.</p>
                <p>Er wordt er een mutatie aangemaakt aan de hand van een persoonlijke publieke en privé sleutel.</p>
                <DeleteComponent />
            </div>
        );
    }
}
