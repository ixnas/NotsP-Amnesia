import React, { Component } from 'react';
import { BlocksController } from '../controllers/BlocksController';
var JSONPretty = require('react-json-pretty');

export class BlockContents extends Component {
  static displayName = BlockContents.name;

  constructor(props){
    super(props)
    this.state = {
      controller: new BlocksController(),
      encryptedData: "",
      blockDefinition: "",
      blockContents: ""
    }
  }

  componentDidMount = () => {
    this.getBlockContent(this.props.blockHash);
    this.getBlockDefinition(this.props.blockHash);
    this.getBlockContents(this.props.blockHash);
  }

  getBlockContent = async (blockHash) => {
    let encryptedData = await this.state.controller.getBlockContent(blockHash)
    this.setState({encryptedData: encryptedData});
    return encryptedData;
  }

  getBlockContents = async (blockHash) => {
    let blockContents = await this.state.controller.getBlockContents(blockHash)
    console.log(blockContents);
    this.setState({blockContents: blockContents});
    return blockContents
  }

  getBlockDefinition = async (blockHash) => {
    let blockDefinition = await this.state.controller.getBlockDefinition(blockHash)
    this.setState({blockDefinition: blockDefinition});
    return blockDefinition;
  }

  render() {
    return (
      <div>
      <p> block hash </p>
      <JSONPretty id="blockHash" data={this.props.blockHash} />
      <p> block data </p>
      <JSONPretty id="blockData" data={this.state.encryptedData} />
      <p> block definition </p>
      <JSONPretty id="blockDefinition" data={this.state.blockDefinition} />
      <p> block contents </p>
      <JSONPretty id="blockContents" data={this.state.blockContents} />
      </div>
    )
  }
}
