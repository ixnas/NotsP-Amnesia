import { Definition } from "../objects/Definition";

export class DefinitionController {
    
    Validate() {
        // Validate the inputValue
    }

    SetDataToDefinition(value) {
        this.Definition = new Definition();
        
        this.Definition.SetPreviousDefinitionHash();
        this.Definition.HashTheData(value);
    }

    SendDefinition() {

        if (this.isValidated) {
            // Post new definition to Web API
            fetch('https://localhost:5001/definitions', {
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