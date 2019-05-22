import { Definition } from "../objects/Definition";
import { KeyHelper } from "../components/functions/keyHelper";

export class DefinitionController {


    Validate() {
        // Validate the inputValue
    }

    SetDataToDefinition(value) {

        this.Validate(value);
        
        this.Definition = new Definition();
        this.Definition.SetDefinition(value, new KeyHelper().getKeys());
    }
}