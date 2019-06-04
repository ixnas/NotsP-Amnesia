const superagent = require('superagent');

export class BlocksController {

  getAllBlocks = async () => {
    const blocks = await superagent.get('http://localhost:5000/blocks/');
    return blocks.body;
  }

  getBlockByHash = async (blockHash) => {
    await superagent
    .get('http://localhost:5000/blocks/' + blockHash)
    .end((err, res) => {
      return res.body
    })
  }

  getDefinitionData = async (definitionDataHash) => {
    const data = await superagent.get('http://localhost:5000/data/' + definitionDataHash + '/data');
    return atob(data.body);  }

  getBlockContentWithDefinitionHash = async (definitionHash) => {
    const definitions = await superagent.get('http://localhost:5000/definitions/' + definitionHash);
    return definitions.body
  }

  getBlockContents = async (blockHash) => {
    const blocks = await superagent.get('http://localhost:5000/blocks/' + blockHash);
    const content = await superagent.get('http://localhost:5000/contents/' + blocks.body.content);
    return content.body;
  }

  getBlockDefinition = async (blockHash) => {
    const blocks = await superagent.get('http://localhost:5000/blocks/' + blockHash);
    const content = await superagent.get('http://localhost:5000/contents/' + blocks.body.content);
    const definitions = await superagent.get('http://localhost:5000/definitions/' + content.body.definitions[0]);
    return definitions.body;
  }

}
