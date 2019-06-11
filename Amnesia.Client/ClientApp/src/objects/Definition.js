import { Data } from "./Data";

export class Definition {
    constructor() {
        //default values
        this.DataHash = "";
        this.PreviousDefinition = "";
        this.Signature = null;
        this.Data = new Data();
        this.IsMutable = true; 
        this.IsMutation = false;
        this.Key = "";
    }
}