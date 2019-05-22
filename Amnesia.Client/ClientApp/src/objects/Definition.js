import { Data } from "./Data";

var CBOR = require('cbor');
var SHA256 = require('js-sha256').sha256;

export class Definition {
    constructor() {
        this.DataHash = null;
        this.PreviousDefinitionHash = null;
        this.Signature = null;
        this.Data = new Data();
        this.Meta = {isMutable: true, isMutation: false};
    }

    SetDefinition(input, keyPair) {
        const publicKey = keyPair.publicKey.exportKey("pkcs8-public-pem");

        fetch(`https://localhost:5001/definitions/last`, {
            method: "POST", 
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                publicKey
            })
        })
        .then(response => response.json())
        .then(definition => {
            this.SetDefinitionAndSend(definition, input, keyPair)
        })
        .catch((err) => console.log(err));
    }    

    SetDefinitionAndSend(definition, input, keyPair) {
        
        this.Data.PreviousDefinitionHash = definition.hash;
        this.PreviousDefinitionHash = definition.hash;

        this.SetHashData(input, keyPair);
        console.log(this);

        // Send();
    }

    Sign(keyPair) {
        var message = new Map();

        message.set("DataHash", this.DataHash).set("PreviousDefinitionHash", this.PreviousDefinitionHash).set("Meta", this.Meta);

        const encodedMessage = CBOR.encode(message);

        const privateKey = keyPair.privateKey;
        this.Signature = privateKey.sign(encodedMessage);
        this.Data.Sign(privateKey);
    }

    SetHashData(input, keyPair) {
        var map = new Map();

        this.Data.Blob = input;
        this.Sign(keyPair);

        map
        .set("PreviousDefinitionHash", this.PreviousDefinitionHash)
        .set("Blob", this.Data.Blob)
        .set("Signature", this.Signature)
        .set("Key", keyPair.publicKey.exportKey("pkcs8-public-pem"));

        this.DataHash = SHA256(CBOR.encode(map));
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