import { Data } from "./Data";
import { base64EncArr } from "../components/functions/Hashing";

var cbor = require('cbor');

export class Definition {
    constructor() {
        this.DataHash = "";
        this.PreviousDefinitionHash = ""; // Ophalen met API
        this.Signature = ""; // Lokaal aangemaakt met private key (RSA)
        this.Data = new Data();
    }

    Sign() {
        // TODO: create signature to send.
        // Also sign data attribute
    }

    SetPreviousDefinitionHash() {
        // TODO: get Previous definition hash with API
        // Also set for data attribute
    }

    HashTheData(input) {
        this.Data.SetBlob(input);
        var map = new Map();
    
        this.Sign();

        map.set("Blob", input).set("Signature", this.Signature);
        this.DataHash = base64EncArr(cbor.encode(map));
    }
}