import { Definition } from "../objects/Definition";

export class DefinitionController {

    Validate() {
        // Validate the inputValue
    }

    SetDataToDefinition(value) {

        this.Validate(value);
        
        this.Definition = new Definition();
        this.Definition.SetDefinition(value);
    }
}