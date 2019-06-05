import { Definition } from "../objects/Definition";
import { KeyHelper } from "../components/functions/keyHelper";
import { base64EncArr } from "../components/functions/base64";

var CBOR = require('cbor');
var SHA256 = require('js-sha256').sha256;
var superagent = require('superagent');

export class DefinitionController {

    SetDataToDefinition(value, isMutation = false) {
        this.Definition = new Definition();
        this.KeyPair = new KeyHelper().getKeys();
        this.Input = value;
        this.IsMutation = isMutation;
        this.SetDefinition();
    }

    /**
     * Sets the definition by getting last definition with a public key
     */
    SetDefinition() {
       // const publicKey = this.KeyPair.publicKey.exportKey("pkcs8-public-pem");
        const publicKey = "-----BEGIN PUBLIC KEY-----\nMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhVQrGn7UopzbDrQerAa+\nggfrnCfWhCTCOPOaazJ5zqbk1vrB9RnHgll0EzMe6zGCfekkd581qh42Eu3VzpeX\nqOAv4Ni1+ydwwB+OKrK0/TyunOh/YmjmBGC9HzOuwh6vdfmFhykpjl3VSR2RnI1o\nE0LPLaqsrL0TgysTxIAMZ6YbmW6qsa+lNhdUjRoo2N8UllWN0ODG/W3qZ0Ce3aY0\nIg4vxjp4FneKjki5TC5jY6LbMTevwKTZxd/08g1REuKtBwIJDzVz1GYwH6S+rVOC\nmyoY9PYHIa7tZpQXC34YvDAAr11pvm0Tr0gRXSDSK4KosJOF+hP5tYQ1PeNaIOog\n6wIDAQAB\n-----END PUBLIC KEY-----";
        
        (async () => {
            try {
              const res = await superagent.post('http://127.0.0.1:8080/definitions/last')
              .send({Key: publicKey})
              .set('accept', 'json');

              console.log(res.body.hash);
              this.SetDefinitionAndSend(res.body.hash);
            } catch (err) {
              console.error(err);
            }
          })();
    }

    /**
     * Sets the definition by setting the definition data and hashing the data.
     * 
     * @param {string} hash 
     */
    SetDefinitionAndSend(hash) {

        this.Definition.Data.PreviousDefinitionHash = hash;
        this.Definition.PreviousDefinitionHash = hash;

        if (this.IsMutation) {
            this.Definition.Data.Blob = `DELETE ${this.Input}`;
            this.Definition.IsMutable = false;
            this.Definition.IsMutation = this.IsMutation;

        } else {
            this.Definition.Data.Blob = btoa(this.Input);
        }

        this.SetHashData();

        this.Send();
    }

    /**
     * Sets the datahash for the definitions
     */
    SetHashData() {
        var map = new Map();

        this.SignDefinition();

        map
            .set("PreviousDefinitionHash", this.Definition.Data.PreviousDefinitionHash)
            .set("Blob", this.Definition.Data.Blob)
            .set("Signature", this.Definition.Data.Signature)
            .set("Key", this.KeyPair.publicKey.exportKey("pkcs8-public-pem"));

        this.Definition.DataHash = SHA256(CBOR.encode(map));
    }

    /**
     * Send the definition to the Web API to add it to the blockchain.
     */
    Send() {
        this.Definition.Data.Signature = base64EncArr(this.Definition.Data.Signature);
        this.Definition.Signature = base64EncArr(this.Definition.Signature);

        fetch('http://127.0.0.1:8080/definitions', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Definition: this.Definition,
                Key: this.KeyPair.publicKey.exportKey("pkcs8-public-pem")
            })
        })
            .catch(err => console.error(err));
    }

    /**
     * Signs de definition with the data from the definition
     */
    SignDefinition() {
        var message = new Map();

        message
            .set("Hash", this.Definition.Hash)
            .set("PreviousDefinitionHash", this.Definition.PreviousDefinitionHash)
            .set("IsMutable", this.Definition.IsMutable)
            .set("IsMutation", this.Definition.IsMutation);

        const encodedMessage = CBOR.encode(message);

        const privateKey = this.KeyPair.privateKey;
        this.Definition.Signature = privateKey.sign(encodedMessage);
        this.SignData(privateKey);
    }


    /**
     * Signs the data with data in Data class
     */
    SignData() {
        var message = new Map();

        message
            .set("PreviousDefinitionHash", this.Definition.PreviousDefinitionHash)
            .set("Blob", this.Definition.Data.Blob);

        const encodedMessage = CBOR.encode(message);
        this.Definition.Data.Signature = this.KeyPair.privateKey.sign(encodedMessage);
    }
}