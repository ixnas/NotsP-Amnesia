import React, { Component } from 'react';
import {BlockContents} from '../BlockContents';

export class BlockContent extends Component {
  static displayName = BlockContent.name;



  render() {
    var url = this.props.match.params.blockHash
    return (
      <div>
        <p> Dit is de content die het door u geselecteerde block bevat </p>
        <BlockContents blockHash={url}/>
      </div>
    )
  }
}
