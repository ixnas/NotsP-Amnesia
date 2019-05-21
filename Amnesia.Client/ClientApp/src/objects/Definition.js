import { Data } from "./Data";
import { KeyHelper } from "../components/functions/keyHelper";
const NodeRSA = require('node-rsa');

var cbor = require('cbor');

export class Definition {
    constructor() {
        this.DataHash = "";
        this.PreviousDefinitionHash = "";
        this.Signature = "";
        this.Data = new Data();
        this.Meta = {};
        this.KeyPair = new KeyHelper().getKeys();
    }

    Sign() {
        var message = new Map();

        message.set("DataHash", this.DataHash).set("PreviousDefinitionHash", this.PreviousDefinitionHash).set("Meta", this.Meta);
        const encodedMessage = new cbor().encode(message);

        const privateKey = this.KeyPair.privateKey;
        this.Signature = privateKey.sign(encodedMessage);
    }

    SetDefinition(input) {
        const publicKey = this.KeyPair.publicKey.exportKey("pkcs8-public-pem");
        fetch(`https://localhost:5001/keys/${publicKey}/definitions`)
            .then(response => {
                console.log(response)
                return response.json();
            })
            .then(definition => this.SetDefinitionAndSend(definition, input))
            .catch((err) => console.error(err));
    }

    SetHashData(input) {
        var map = new Map();

        this.Data.Blob = input;
        // this.Sign();

        map
        .set("PreviousDefinitionHash", this.PreviousDefinitionHash)
        .set("Blob", this.Data.Blob)
        .set("Signature", this.Signature)
        .set("Key", this.KeyPair.publicKey);

        this.DataHash = cbor.encode(map);
    }

    SetDefinitionAndSend(definition, input) {
        this.Data.PreviousDefinitionHash = definition.hash;
        this.PreviousDefinitionHash = definition.hash;
        this.SetHashData(input);
        console.log(this);

        // Send();
    }

    Send() {
        fetch('https://localhost:5001/definitions', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                definition: this,
                key: this.KeyPair.publicKey
            })
        })
        .catch(err => console.error(err));
    }
}