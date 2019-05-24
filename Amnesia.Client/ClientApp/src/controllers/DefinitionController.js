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
        this.SetDefinition(value, new KeyHelper().getKeys());
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
                PublicKey: publicKey
            })
        })
            .then(response => response.json())
            .then(definition => {
                this.SetDefinitionAndSend(definition, input, keyPair)
            })
            .catch((err) => console.log(err));
    }

    SetDefinitionAndSend(definition, input, keyPair) {

        this.Definition.Data.PreviousDefinitionHash = definition.hash;
        this.Definition.PreviousDefinitionHash = definition.hash;

        this.SetHashData(input, keyPair);

        this.Send(keyPair);
    }

    Sign(keyPair) {
        var message = new Map();

        message
        .set("Hash", this.Definition.Hash)
        .set("PreviousDefinitionHash", this.Definition.PreviousDefinitionHash)
        .set("Meta", this.Definition.Meta);

        const encodedMessage = CBOR.encode(message);

        const privateKey = keyPair.privateKey;
        this.Definition.Signature = privateKey.sign(encodedMessage);
        this.Definition.Data.Sign(privateKey);
    }

    SetHashData(input, keyPair) {
        var map = new Map();

        this.Definition.Data.Blob = input;
        this.Sign(keyPair);

        map
            .set("PreviousDefinitionHash", this.Definition.PreviousDefinitionHash)
            .set("Blob", this.Definition.Data.Blob)
            .set("Signature", this.Definition.Signature)
            .set("Key", keyPair.publicKey.exportKey("pkcs8-public-pem"));

        this.Definition.Hash = SHA256(CBOR.encode(map));
    }

    Send(keyPair) {
        this.Definition.Data.Signature = base64EncArr(this.Definition.Data.Signature);
        this.Definition.Signature = base64EncArr(this.Definition.Signature);

        const map = new Map();

        map
        .set("Hash", this.Definition.Hash)
        .set("PreviousDefinitionHash", this.Definition.PreviousDefinitionHash)
        .set("Signature", this.Definition.Signature)
        .set("Data", this.Definition.Data)
        .set("Meta", this.Definition.Meta);

        const payload = {
            Definition: this.Definition,
            Key: keyPair.publicKey.exportKey("pkcs8-public-pem")
        };

        console.log(payload)

        fetch('https://localhost:5001/definitions', {
            method: 'POST',
            headers:  {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(payload)
        })
            .catch(err => console.error(err));
    }
}