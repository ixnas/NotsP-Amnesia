import React, { Component } from 'react';
import { Button } from 'reactstrap';
import { BlocksController } from '../controllers/BlocksController';

export class BlocksComponent extends Component {
  static displayName = BlocksComponent.name;

  constructor(props) {
    super(props);
    this.state = {
      controller: new BlocksController(),
      blocks: []
    }
  }

  getBlockContent = async (blockHash) => {
    let encryptedData = await this.state.controller.getBlockContent(blockHash)
    return encryptedData;
  }

  getAllBlocks = async () => {
    let blocks = await this.state.controller.getAllBlocks();
    this.setState({blocks: blocks});
    return blocks
  }

  navigateToBlockContent = async (blockHash) => {
    let x = await this.state.controller.getBlockContent(blockHash);
    console.log(x);
    window.location="http://localhost:3000/blockContent/"+ blockHash
  }

  render() {
      return (
          <div>
            <div>
            <Button onClick={() => this.getAllBlocks()}> Haal alle blocks op uit de chain </Button>
            </div>
            <div>
             {this.state.blocks.map(block => <p key={block.hash} onClick={() => this.navigateToBlockContent(block.hash)}> {JSON.stringify({block})} </p>)}
            </div>
          </div>

      );
  }

}
