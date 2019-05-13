export class Data {
    constructor() {
        this.PreviousDefinitionHash = "";
        this.Signature = "";
        this.Blob = {};
    }

    SetPreviousDefinitionHash(hash) {
        this.PreviousDefinitionHash = hash;
    }

    SetSignature(signature) {
        this.Signature = signature;
    }

    SetBlob(blob) {
        this.Blob = blob;
    }
}