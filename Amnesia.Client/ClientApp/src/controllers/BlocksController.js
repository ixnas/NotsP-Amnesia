import {Block} from "../objects/Block";
const superagent = require('superagent');

export class BlocksController {

  // hoe haal ik de public key op van het blok?
  //

  getAllBlocks = () => {
    superagent
    .get('http://localhost:5000/blocks/')
    .end((err, res) => {
      console.log(res.body);
    })
  }

  getBlockByHash = async (blockHash) => {
    await superagent
    .get('http://localhost:5000/blocks/' + blockHash)
    .end((err, res) => {
      return res.body
    })
  }

  getBlockContent = async (blockHash) => {
    const blocks = await superagent.get('http://localhost:5000/blocks/' + blockHash);
    const content = await superagent.get('http://localhost:5000/contents/' + blocks.body.content);
    const definitions = await superagent.get('http://localhost:5000/definitions/' + content.body.definitions[0]);
    const data = await superagent.get('http://localhost:5000/data/' + definitions.body.dataHash + '/data');
    return atob(data.body);
  }

}
