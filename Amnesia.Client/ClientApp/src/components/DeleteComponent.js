import React, { Component } from 'react';
import { Button } from 'reactstrap';
import Select from 'react-select'

export class DeleteComponent extends Component {
    static displayName = DeleteComponent.name;

    constructor(props) {
        super(props);

        this.state = {
            data: [{ id: 0, name: "test", data: "test" },
                { id: 1, name: "test2", data: "test2" },
                { id: 2, name: "test3", data: "test3"}
            ],
            selectedBlock: {}
        };
    }

    render() {
        const options = ((this.state.data));

        function logChange(val) {
            console.log("Selected: " + JSON.stringify(val));
        }

        let dataDropdown = options.map(option => {
            return { label: option.name, value: option.data };

        })

        return (
            <div className="container">
                <div className="row">
                    <div className="col-md-4">
                        <p> Gebruik de dropdown om data te selecteren en te verwijderen </p>
                    </div>

                    <div className="col-md-4">
                        <Select options={dataDropdown} onChange={logChange}/>
                    </div>

                    <div className="col-md-4">
                        <Button> Verwijder data </Button>
                    </div>
                </div>
            </div>


        );
    }
}