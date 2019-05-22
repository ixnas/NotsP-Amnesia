var cbor = require('cbor');

export class Data {
    constructor() {
        this.PreviousDefinitionHash = "";
        this.Signature = "";
        this.Blob = "";
    }

    Sign(privateKey) {
        var message = new Map();
        message.set("PreviousDefinitionHash", this.PreviousDefinitionHash).set("Blob", this.Blob);
        const encodedMessage = cbor.encode(message);
        this.Signature = privateKey.sign(encodedMessage);
    }
}