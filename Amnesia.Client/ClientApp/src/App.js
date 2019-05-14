import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './Layout';
import { Home } from './components/Pages/Home';
const NodeRSA = require('node-rsa');

export default class App extends Component {
    static displayName = App.name;

    state = {
        privateKey: '',
        publicKey: ''
    }


    componentDidMount() {
        const key = new NodeRSA({ b: 512 });
        const keySavedInLocal = localStorage.getItem('keySaveState');
        if (!keySavedInLocal) {
            this.keySaver(key)
        } else {
            const privateKey = new NodeRSA(localStorage.getItem('pemStringPrivate'));
            const publicKey = new NodeRSA(localStorage.getItem('pemStringPublic'));
            this.setState({ publicKey, privateKey });
        }
    }

    /**
     * This function saves the keys into localstorage
     * param key = the NodeRSA key that gets generated when app.js mounts
    **/

    keySaver = (key) => {
        const keyPair = key.generateKeyPair();
        localStorage.setItem('keySaveState', 'true');
        localStorage.setItem('pemStringPublic', keyPair.exportKey("public"));
        localStorage.setItem('pemStringPrivate', keyPair.exportKey("private"));

    }

    /**
     * This function encrypts a message with the pemString of a private key
     * param pemStringPrivate = the pem string of the private key saved in localstorage
     * param message = the message that you want to encrypt
     **/

    createEncryptedMessagePrivateKey = (pemStringPrivate, message) => {
        const privateKey = new NodeRSA(pemStringPrivate);
        const encryptedMessage = privateKey.encryptPrivate(message, 'base64');
        return encryptedMessage;
    }

    /**
     * This function decrypts a message that is encrypted with the corresponding private key
     * param pemStringPublic = the pem string of the public key saved in localstorage
     * param message = the message that you want to decrypt
     **/

    decryptMessagePublicKey = (pemStringPublic, message) => {
        const publicKey = new NodeRSA(pemStringPublic);
        const decryptedMessage = publicKey.decryptPublic(message, 'utf8');
        return decryptedMessage;
    }

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
            </Layout>
        );
    }
}
