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
import Categories from './components/Categories';
import Category from './components/Category';

export default () => (
  <main>
    <Route path='/' component={Home} />
    <Switch>
      <Route exact path='/login' component={Login} />
      <Route exact path='/register' component={Register} />
      <Route exact path='/categories' component={Categories} />
      <Route exact path='/categories/:id' component={Category} />
    </Switch>
  </main>
);
