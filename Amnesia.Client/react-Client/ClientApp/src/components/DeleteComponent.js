import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { log } from 'util';

export class DeleteComponent extends Component {
    static displayName = DeleteComponent.name;

    constructor(props) {
        super(props);

        this.state = {
            data: [{ id: 0, name: "test", data: "test" }, { id: 1, name: "test2", data: "test2" }],
            selectedBlock: {}
        };
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event) {
        this.setState({ value: event.target.value });
    }

    handleSubmit(event) {
        alert('Your favorite flavor is: ' + this.state.value);
        event.preventDefault();
    }

    render() {

        let dataDropdown = this.state.data.map(result => {
            console.log(result);
            return result
        })

        return (
            <div className="container">
                <div className="row">
                    <div className="col-md-4">
                        <p> Gebruik de dropdown om data te selecteren en te verwijderen </p>
                        {console.log({ dataDropdown })}
                    </div>

                    <div className="col-md-4">
                        <select onChange={this.handleChange}>
                            <option> {JSON.stringify(dataDropdown[0].name)} </option>
                        </select>
                    </div>



                    <div className="col-md-4">
                        <Button> Verwijder data </Button>
                    </div>
                </div>
            </div>


        );
    }
}