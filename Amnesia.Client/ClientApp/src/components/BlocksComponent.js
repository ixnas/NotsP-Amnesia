import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { BlocksController } from '../controllers/BlocksController';
var JSONPretty = require('react-json-pretty');

export class BlocksComponent extends Component {
    static displayName = BlocksComponent.name;

    constructor(props) {
        super(props);
        this.state = {
            controller: new BlocksController(),
            blocks: []
        }
    }

    getAllBlocks = async () => {
        let blocks = await this.state.controller.getAllBlocks();
        this.setState({ blocks: blocks });
        return blocks
    }

    navigateToBlockContent = async (blockHash) => {
        window.location = "http://localhost:3000/blockContent/" + blockHash
    }

    render() {
        return (
            <div>
                <div>
                    <Button onClick={() => this.getAllBlocks()}> Haal alle blocks op uit de chain </Button>
                </div>
                <div>
                    {this.state.blocks.map(block => <JSONPretty id="blocks" key={block.hash} onClick={() => this.navigateToBlockContent(block)} data={block} />)}
                </div>
            </div>

        );
    }

}
