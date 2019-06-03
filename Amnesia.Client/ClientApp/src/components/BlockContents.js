import React, { Component } from 'react';
import { BlocksController } from '../controllers/BlocksController';

export class BlockContents extends Component {
  static displayName = BlockContents.name;

  constructor(props){
    super(props)
    this.state = {
      controller: new BlocksController(),
      encryptedData: "",
      blockDefinition: ""
    }
  }

  componentDidMount = () => {
    this.getBlockContent(this.props.blockHash);
    this.getBlockDefinition(this.props.blockHash);
  }

  getBlockContent = async (blockHash) => {
    let encryptedData = await this.state.controller.getBlockContent(blockHash)
    this.setState({encryptedData: encryptedData});
    return encryptedData;
  }

  getBlockDefinition = async (blockHash) => {
    let blockDefinition = await this.state.controller.getBlockDefinition(blockHash)
    this.setState({blockDefinition: blockDefinition});
    return blockDefinition;
  }

  render() {
    return (
      <div>
      <p> De geselecteerde blockHash {this.props.blockHash} </p>
      <p> De blockDefinition is {JSON.stringify(this.state.blockDefinition)} </p>
      <p> De data {JSON.stringify(this.state.encryptedData)} </p>
      </div>
    )
  }
}
