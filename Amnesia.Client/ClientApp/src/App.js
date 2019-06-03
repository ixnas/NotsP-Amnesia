import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './Layout';
import { Home } from './components/Pages/Home';
import { DeleteDefinition } from './components/Pages/DeleteDefinition'
import { AddDefinition } from './components/Pages/AddDefinition'
import { KeyHelper } from './components/functions/keyHelper.js'

export default class App extends Component {
    static displayName = App.name;

    state = {
        privateKey: '',
        publicKey: ''
    }

    componentDidMount() {
        const keyHelper = new KeyHelper();

        const keySavedInLocal = localStorage.getItem('keySaveState');

        if (!keySavedInLocal) {
            keyHelper.keySaver();
        }
    }

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Home} />
                <Route exact path='/delete' component={DeleteDefinition} />
                <Route exact path='/add' component={AddDefinition} />
            </Layout>
        );
    }
}
