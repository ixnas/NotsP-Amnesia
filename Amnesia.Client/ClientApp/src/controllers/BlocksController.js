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
        const data = await superagent.get(await this.getIPToSendRequestTo() + '/definitions/' + definitionDataHash);
        return data.body;
    }

    getDefinitionContent = async (definitionHash) => {
        try {
            const base64Data = await superagent.get(await this.getIPToSendRequestTo() + '/definitions/' + definitionHash + '/data/blob')
            return JSON.stringify(base64Data.text);
        } catch(error) {
            return ("Data is mogelijk verwijderd");
        }

    }

    getBlockContentWithBlockHash = async (blockHash) => {
        const definitions = await superagent.get(await this.getIPToSendRequestTo() + '/blocks/' + blockHash);
        return definitions.body
    }

    getBlockContents = async (blockHash) => {
        const ip = await this.getIPToSendRequestTo();
        const blocks = await superagent.get(ip + '/blocks/' + blockHash + "/content");
        return blocks.body;
    }



}
