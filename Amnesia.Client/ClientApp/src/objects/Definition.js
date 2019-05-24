import { Data } from "./Data";

export class Definition {
    constructor() {
        this.Hash = "";
        this.PreviousDefinitionHash = "";
        this.Signature = null;
        this.Data = new Data();
        this.Meta = { isMutable: true, isMutation: false };
    }
}