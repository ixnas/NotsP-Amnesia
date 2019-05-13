import { Definition } from "../objects/Definition";

export class DefinitionController {

    constructor(inputValue) {
        this.inputValue = inputValue;
        this.Definition = new Definition();
        this.isValidated = false;
    }

    Validate() {
        // Validate the inputValue
        this.isValidated = true;
        return true;
    }

    SetDataToDefinition() {
        if (this.isValidated) {
            this.Definition.SetPreviousDefinitionHash();
            this.Definition.HashTheData(this.inputValue);
        }
    }

    SendDefinition() {

        if (this.isValidated) {
            // Post new definition to Web API
            fetch('', { // FIX: undertermined URL
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    definition: this.Definition
                })
            })
        }
    }
}