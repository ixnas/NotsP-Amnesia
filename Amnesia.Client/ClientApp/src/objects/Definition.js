import { Data } from "./Data";
import { base64EncArr } from "../components/functions/Hashing";

var cbor = require('cbor');

export class Definition {
    constructor() {
        this.DataHash = "";
        this.PreviousDefinitionHash = "";
        this.Signature = ""; // Lokaal aangemaakt met private key (RSA)
        this.Data = new Data();
    }

    Sign() {
        // TODO: create signature to send.
        // Also sign data attribute
    }

    SetPreviousDefinitionHash(hash) {
        this.PreviousDefinitionHash = hash;
    }

    SetDefinition(input) {
        fetch("https://localhost:5001/definitions/last")
            .then(response => response.json())
            .then(definition => this.SetDefinitionAndSend(definition, input))
            .catch((err) => console.error(err));
    }

    SetHashData(input) {
        var map = new Map();

        this.Data.SetBlob(input);
        this.Sign();

        map.set("Blob", input).set("Signature", this.Signature);
        this.DataHash = base64EncArr(cbor.encode(map));
    }

    SetDefinitionAndSend(definition, input) {
        this.Data.SetPreviousDefinitionHash(definition.hash);
        this.SetPreviousDefinitionHash(definition.hash);
        this.SetHashData(input);

        console.log(JSON.stringify({
            definition: this
        }));
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
                definition: this
            })
        })
        .catch(err => console.error(err));
    }
}