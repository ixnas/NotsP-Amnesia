import { Definition } from "../objects/Definition";
import { KeyHelper } from "../components/functions/keyHelper";

import { base64EncArr } from "../components/functions/base64";

var CBOR = require('cbor');
var SHA256 = require('js-sha256').sha256;

export class DefinitionController {

    Validate() {
        // Validate the inputValue
    }

    SetDataToDefinition(value) {

        this.Validate(value);
        
        this.Definition = new Definition();
        this.KeyPair = new KeyHelper().getKeys();
        this.Input = value;
        this.SetDefinition(value);
    }

    SetDefinition() {
        const publicKey = this.KeyPair.publicKey.exportKey("pkcs8-public-pem");

        fetch(`https://localhost:5001/definitions/last`, {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                PublicKey: publicKey
            })
        })
            .then(response => response.json())
            .then(definition => {
                this.SetDefinitionAndSend(definition.hash)
            })
            .catch((err) => console.log(err));
    }

    SetDefinitionAndSend(hash) {

        this.Definition.Data.PreviousDefinitionHash = hash;
        this.Definition.PreviousDefinitionHash = hash;

        this.SetHashData();

        this.Send();
    }

    SignDefinition() {
        var message = new Map();

        message
        .set("Hash", this.Definition.Hash)
        .set("PreviousDefinitionHash", this.Definition.PreviousDefinitionHash)
        .set("Meta", this.Definition.Meta);

        const encodedMessage = CBOR.encode(message);

        const privateKey = this.KeyPair.privateKey;
        this.Definition.Signature = privateKey.sign(encodedMessage);
        this.SignData(privateKey);
    }

    SetHashData() {
        var map = new Map();

        this.Definition.Data.Blob = this.Input;
        this.SignDefinition();

        map
            .set("PreviousDefinitionHash", this.Definition.PreviousDefinitionHash)
            .set("Blob", this.Definition.Data.Blob)
            .set("Signature", this.Definition.Signature)
            .set("Key", this.KeyPair.publicKey.exportKey("pkcs8-public-pem"));

        this.Definition.Hash = SHA256(CBOR.encode(map));
    }

    Send() {
        this.Definition.Data.Signature = base64EncArr(this.Definition.Data.Signature);
        this.Definition.Signature = base64EncArr(this.Definition.Signature);

        fetch('https://localhost:5001/definitions', {
            method: 'POST',
            headers:  {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Definition: this.Definition,
                Key: this.KeyPair.publicKey.exportKey("pkcs8-public-pem")
            })
        })
            .catch(err => console.error(err));
    }

    SignData(privateKey) {
        var message = new Map();
        message.set("PreviousDefinitionHash", this.Definition.PreviousDefinitionHash).set("Blob", this.Definition.Data.Blob);
        const encodedMessage = CBOR.encode(message);
        this.Definition.Data.Signature = privateKey.sign(encodedMessage);
    }
}