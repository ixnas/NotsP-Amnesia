export class Data {
    constructor() {
        this.PreviousDefinitionHash = "";
        this.Signature = "";
        this.Blob = "";
    }

    Sign() {
        var message = new Map();
        message.set("PreviousDefinitionHash", this.PreviousDefinitionHash).set("Blob", this.Blob);
    }
}