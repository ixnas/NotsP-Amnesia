import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { BlocksController } from '../controllers/BlocksController';
var JSONPretty = require('react-json-pretty');

export class BlockContents extends Component {
    static displayName = BlockContents.name;

    constructor(props) {
        super(props)
        this.state = {
            controller: new BlocksController(),
            encryptedData: "",
            blockDefinition: "",
            blockContents: ""
        }
    }

    getBlockContents = async (blockHash) => {
        let blockContents = await this.state.controller.getBlockContents(blockHash)
        this.setState({ blockContents: blockContents });
        return blockContents
    }

    getDefinitionFromBlock = async (e) => {
        e.preventDefault();
        let clickedDefinitionHash = JSON.parse(e.target.innerText)
        let blockDefinitionClicked = await this.state.controller.getDefinitionData(clickedDefinitionHash);
        let blockContent = await this.state.controller.getDefinitionContent(clickedDefinitionHash);
        this.setState({ blockDefinition: blockDefinitionClicked, encryptedData: blockContent });
        return blockDefinitionClicked
    }

    render() {
        return (
            <div>
                <div>
                    <Button onClick={() => this.getBlockContents(this.props.blockHash)}> Haal block content op </Button>
                </div>
                <p> block hash </p>
                <JSONPretty id="blockHash" data={this.props.blockHash} />
                <p> block data </p>
                <JSONPretty id="blockData" data={this.state.encryptedData} />
                <p> block definition </p>
                <JSONPretty id="blockDefinition" data={this.state.blockDefinition} />
                <p> block contents </p>
                <JSONPretty id="blockContents" onClick={(e) => this.getDefinitionFromBlock(e)} data={this.state.blockContents} />
            </div>
        )
    }
}
