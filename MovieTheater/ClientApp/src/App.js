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
import Categories from './components/Categories/Categories';
import Category from './components/Categories/Category';
import 'react-toastify/dist/ReactToastify.css';
import { ToastContainer } from 'react-toastify';
import Movies from './components/Movies/Movies';
import Cinemas from './components/Cinemas/Cinemas';

export default function () {
  return (
    <main>
      <Route path='/' component={Home} />
      <Switch>
        <Route exact path='/login' component={Login} />
        <Route exact path='/register' component={Register} />
        <PrivateRoute exact path='/categories' component={Categories} />
        <PrivateRoute exact path='/categories/:id' component={Category} />
        <PrivateRoute exact path="/movies" component={Movies} />
        <PrivateRoute exact path="/cinemas" component={Cinemas} />
      </Switch>
      <ToastContainer
        position="top-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnVisibilityChange
        draggable
        pauseOnHover
      />
    </main>
  );
}
