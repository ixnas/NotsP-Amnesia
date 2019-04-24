import React, { Component } from 'react';

export class InformationComponent extends Component {
    static displayName = InformationComponent.name

    constructor(props) {
        super(props);
        this.state = {

        }
    }

    render() {
        return (
            <div>
                <h1> Hier komt straks die informatie die geretourneerd wordt </h1>
            </div>

        );

    }
}