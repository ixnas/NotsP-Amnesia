import React, { Component } from 'react';
import { Button } from 'reactstrap';
import Select from 'react-select'



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
    // stuurt objecten terug
    render() {
        const options = [
            { value: 'chocolate', label: 'Chocolate' },
            { value: 'strawberry', label: 'Strawberry' },
            { value: 'vanilla', label: 'Vanilla' }
        ];

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
                        <Select options={options} />
                    </div>



                    <div className="col-md-4">
                        <Button> Verwijder data </Button>
                    </div>
                </div>
            </div>


        );
    }
}