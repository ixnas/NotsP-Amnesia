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
        const publicKey = this.KeyPair.publicKey.exportKey("pkcs8-public-pem");
        
        if (this.Input.includes("DELETE")){
            console.log(true)
            var def = this.Input.substring(7);
            console.log(def);
            var json = {
                Blob : btoa(this.Input),
                PrivateKey : this.KeyPair.privateKey.exportKey("pkcs1-private-pem"),
                PublicKey : this.KeyPair.publicKey.exportKey("pkcs8-public-pem"),
                DefinitionHash : def
            };
        } else {
            console.log(false)
            var json = {
                Blob : btoa(this.Input),
                PrivateKey : this.KeyPair.privateKey.exportKey("pkcs1-private-pem"),
                PublicKey : this.KeyPair.publicKey.exportKey("pkcs8-public-pem")
            };
        }

        console.log(json);
        console.log(btoa(this.Input));
        //const res = await superagent.post('http://127.0.0.1:8080/definitions/last')        
        //  .send({Key: publicKey})
        (async () => {
            try {
              const res  = await superagent.post('http://127.0.0.1:8081/definitions/easy')
                    .send(json)
                    .set('accept', 'json');
              //this.SetDefinitionAndSend(res.body.hash);
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
        this.Definition.Data.PreviousDefinition = hash;
        this.Definition.PreviousDefinition = hash;

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
            .set("PreviousDefinitionHash", this.Definition.Data.PreviousDefinition)
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
        this.Definition.Key = this.KeyPair.publicKey.exportKey("pkcs8-public-pem")
        this.Definition.Data.Key = this.KeyPair.publicKey.exportKey("pkcs8-public-pem")
        fetch('http://127.0.0.1:8080/definitions', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Definition: this.Definition,
                Data: this.Definition.Data
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
            .set("DataHash", this.Definition.DataHash)
            .set("PreviousDefinitionHash", this.Definition.PreviousDefinition)
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
            .set("PreviousDefinitionHash", this.Definition.PreviousDefinition)
            .set("Blob", this.Definition.Data.Blob);

        const encodedMessage = CBOR.encode(message);
        this.Definition.Data.Signature = this.KeyPair.privateKey.sign(encodedMessage);
    }
}