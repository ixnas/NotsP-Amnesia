import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div>
                <h1>Welkom bij de client van Amnesia </h1>
                <p> Met behulp van deze client kunt u het Proof of Concept testen die het onderzoek voor groep Data Protection ondersteunt. </p>
                <p>In deze client kunt u blocks van de blockchain inzien, nieuwe gegevens toevoegen en tot slot ook gegevens verwijderen uit de blockchain met ons ontwerp.</p>
            </div>
        );
    }
}
