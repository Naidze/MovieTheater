import React from 'react';
import {
  BrowserRouter,
  Route,
  Switch
} from 'react-router-dom';
import Home from './components/Home';
import Login from './components/Authentication/Login';
import Register from './components/Authentication/Register';
import { PrivateRoute } from './components/Authentication/PrivateRoute';

export default () => (
  <BrowserRouter>
    <Switch>
      <PrivateRoute exact path='/' component={Home} />
      <Route exact path='/login' component={Login} />
      <Route exact path='/register' component={Register} />
    </Switch>
  </BrowserRouter>
);
