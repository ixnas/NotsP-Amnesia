import React, { Component } from 'react';
import { DeleteComponent } from '../DeleteComponent';
import { AddComponent } from '../AddComponent';

export class Home extends Component {
    static displayName = Home.name;

    render() {
        return (
            <div>
                <h1> Welkom bij de client van Amnesia </h1>
                <p> Met behulp van deze client kunt u inzicht krijgen in de taken die achter de schermen gebeuren in de blockchain.</p>
                <DeleteComponent />
                <AddComponent />
            </div>
        );
    }
}
