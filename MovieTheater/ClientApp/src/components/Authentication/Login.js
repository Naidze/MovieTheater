import React, { PureComponent } from 'react';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
import Link from '@material-ui/core/Link';
import { Link as RouteLink } from 'react-router-dom';
import Paper from '@material-ui/core/Paper';
import Box from '@material-ui/core/Box';
import Grid from '@material-ui/core/Grid';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import { Copyright } from '../../utils/helpers';
import { Formik, Form } from 'formik';
import { loginValidation } from '../../utils/validation';
import { withStyles } from '@material-ui/core/styles';
import axios from 'axios';
import { isAuth } from '../../utils/auth';

const styles = theme => ({
	root: {
		height: '100vh',
	},
	image: {
		backgroundImage: 'url(images/movie.jpg)',
		backgroundRepeat: 'no-repeat',
		backgroundColor:
			theme.palette.type === 'dark' ? theme.palette.grey[900] : theme.palette.grey[50],
		backgroundSize: 'cover',
		backgroundPosition: 'left',
	},
	paper: {
		margin: theme.spacing(8, 4),
		display: 'flex',
		flexDirection: 'column',
		alignItems: 'center',
	},
	avatar: {
		margin: theme.spacing(1),
		backgroundColor: theme.palette.secondary.main,
	},
	form: {
		width: '100%', // Fix IE 11 issue.
		marginTop: theme.spacing(1),
	},
	submit: {
		margin: theme.spacing(3, 0, 2),
	},
});

const submitLogin = values => {
	console.log(values);
	axios.post("/api/auth/token", values)
		.then(r => {
			localStorage.setItem("access_token", r.data)
		})
		.then(r => window.location.reload())
		.catch(err => console.log(err));
}

class Login extends PureComponent {
	componentDidMount() {
		console.log(isAuth());
		if (isAuth()) {
			// window.location.replace('/');
		}
	}

	render() {
		const {
			classes
		} = this.props;

		return (
			<Grid container component="main" className={classes.root}>
				<CssBaseline />
				<Grid item xs={false} sm={4} md={7} className={classes.image} />
				<Grid item xs={12} sm={8} md={5} component={Paper} elevation={6} square>
					<div className={classes.paper}>
						<Avatar className={classes.avatar}>
							<LockOutlinedIcon />
						</Avatar>
						<Typography component="h1" variant="h5">
							Sign in
          </Typography>
						<Formik
							initialValues={{ username: '', password: '' }}
							validationSchema={loginValidation}
							onSubmit={(values, actions) => {
								actions.setSubmitting(true);
								submitLogin(values);
							}}
							render={(formProps) => {
								const { values, handleChange, setFieldTouched } = formProps;
								const change = (name, e) => {
									e.persist();
									handleChange(e);
									setFieldTouched(name, true, false);
								};
								return (
									<Form className={classes.form}>
										<TextField
											variant="outlined"
											margin="normal"
											required
											fullWidth
											id="username"
											label="Username"
											name="username"
											autoComplete="username"
											autoFocus
											value={values.username}
											onChange={change.bind(null, "username")}
										/>
										<TextField
											variant="outlined"
											margin="normal"
											required
											fullWidth
											name="password"
											label="Password"
											type="password"
											id="password"
											autoComplete="current-password"
											value={values.password}
											onChange={change.bind(null, "password")}
										/>
										<Button
											type="submit"
											fullWidth
											variant="contained"
											color="primary"
											className={classes.submit}
										>
											Sign In
            </Button>
										<Grid container>
											<Grid item>
												<RouteLink to="/register">
													<Link variant="body2">
														{"Don't have an account? Sign Up"}
													</Link>
												</RouteLink>
											</Grid>
										</Grid>
										<Box mt={5}>
											<Copyright />
										</Box>
									</Form>
								);
							}}
						/>
					</div>
				</Grid>
			</Grid>
		);
	}
}

export default withStyles(styles)(Login);