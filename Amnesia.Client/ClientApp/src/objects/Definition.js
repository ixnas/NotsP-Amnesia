import { Data } from "./Data";

export class Definition {
    constructor() {
        //default values
        this.DataHash = "";
        this.PreviousDefinitionHash = "";
        this.Signature = null;
        this.Data = new Data();
        this.IsMutable = true; 
        this.IsMutation = false;
    }
}