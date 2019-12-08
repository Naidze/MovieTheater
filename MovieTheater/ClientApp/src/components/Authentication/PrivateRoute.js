import React from 'react';
import { Route, Redirect } from 'react-router';
import { authenticate } from '../../utils/networkFunctions';
import { toast } from 'react-toastify';
import { isAuth } from '../../utils/auth';

export const fakeAuth = {
	isAuthenticated: sessionStorage.getItem("IS_AUTHENTICATED") === 'true' || isAuth(),
	authenticate(cb) {
		authenticate()
			.then(r => {
				sessionStorage.setItem("IS_AUTHENTICATED", true);
				window.location.reload();
			})
			.catch(err => {
				toast.err(err.message);
				sessionStorage.removeItem("IS_AUTHENTICATED");
				localStorage.removeItem("ACCESS_TOKEN");
			});
		setTimeout(cb, 100);
	},
	signout(cb) {
		sessionStorage.removeItem("IS_AUTHENTICATED");
		localStorage.removeItem("ACCESS_TOKEN");
		window.location.reload();
		setTimeout(cb, 100)
	}
}

export const PrivateRoute = ({ component: Component, ...rest }) => (
	<Route {...rest} render={(props) => (
		fakeAuth.isAuthenticated
			? <Component {...props} />
			: <Redirect to={{
				pathname: '/login',
				state: { from: props.location }
			}} />
	)} />
)