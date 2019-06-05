const superagent = require('superagent');

export class BlocksController {

    getIPToSendRequestTo = async () => {
        const nodeIP = await document.getElementById('inputIP').value
        if (nodeIP === "") {
            alert('Geen IP adress meegegeven, http://localhost:8080 is gebruikt voor dit request');
            return 'http://localhost:8080'
        } else {
            return nodeIP;
        }
    }

    getAllBlocks = async () => {
        const connectionString = await this.getIPToSendRequestTo() + "/blocks/";
        console.log(connectionString);
        const blocks = await superagent.get(connectionString);
        return blocks.body;
    }

    getBlockByHash = async (blockHash) => {
        await superagent
            .get(await this.getIPToSendRequestTo() + blockHash)
            .end((err, res) => {
                return res.body
            })
    }

    getDefinitionData = async (definitionDataHash) => {
        const data = await superagent.get(await this.getIPToSendRequestTo() +'/data/' + definitionDataHash + '/data');
        return atob(data.body);
    }

    getBlockContentWithDefinitionHash = async (definitionHash) => {
        const definitions = await superagent.get(await this.getIPToSendRequestTo() +  '/definitions/' + definitionHash);
        return definitions.body
    }

    getBlockContents = async (blockHash) => {
        const ip = await this.getIPToSendRequestTo();
        const blocks = await superagent.get(ip + '/blocks/' + blockHash);
        const content = await superagent.get(ip + '/contents/' + blocks.body.content);
        return content.body;
    }

    getBlockDefinition = async (blockHash) => {
        const ip = await this.getIPToSendRequestTo();
        const blocks = await superagent.get(ip + '/blocks/' + blockHash);
        const content = await superagent.get(ip + '/contents/' + blocks.body.content);
        const definitions = await superagent.get(ip + '/definitions/' + content.body.definitions[0]);
        return definitions.body;
    }

}
