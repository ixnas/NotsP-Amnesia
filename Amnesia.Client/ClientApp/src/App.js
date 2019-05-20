import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './Layout';
import { Home } from './components/Pages/Home';
import {KeyHelper} from './keyHelper.js'
const NodeRSA = require('node-rsa');

export default class App extends Component {
    static displayName = App.name;

    state = {
        privateKey: '',
        publicKey: ''
    }


    componentDidMount() {
        const key = new NodeRSA({ b: 512 });
        const key2 = new KeyHelper(key);
        const keySavedInLocal = localStorage.getItem('keySaveState');
        if (!keySavedInLocal) {
            this.keySaver(key)
        } else {
            const privateKey = new NodeRSA(localStorage.getItem('pemStringPrivate'));
            const publicKey = new NodeRSA(localStorage.getItem('pemStringPublic'));
            this.setState({ publicKey, privateKey });
        }
    }


    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
            </Layout>
        );
    }
}
