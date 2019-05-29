import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './Layout';
import { Home } from './components/Pages/Home';
import { KeyHelper } from './components/functions/keyHelper.js'
import { BlocksController } from './controllers/BlocksController.js'
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

    testGetBlockContent = async () => {
      let test3 = new BlocksController;
      let test2 = await test3.getBlockContent("7d72f32c4a1d9932beccb343f1f0c51dd14aa730975a06f28a9ad56e4b597f3a")
      console.log(test2);
      return test2;
    }

    render() {
      const test = new BlocksController;
      test.getAllBlocks();
      let encodedData = this.testGetBlockContent();
      console.log(encodedData);
        return (
            <Layout>
                <Route exact path='/' component={Home} />
            </Layout>
        );
    }
}
