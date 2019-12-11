import React, { useEffect } from 'react';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
import Link from '@material-ui/core/Link';
import { Link as RouteLink } from 'react-router-dom';
import Grid from '@material-ui/core/Grid';
import Box from '@material-ui/core/Box';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import { Copyright } from '../../utils/helpers';
import { Formik, Form } from 'formik';
import { register } from '../../utils/networkFunctions';
import { toast } from 'react-toastify';
import { fakeAuth } from './PrivateRoute';

const useStyles = makeStyles(theme => ({
	paper: {
		marginTop: theme.spacing(8),
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
		marginTop: theme.spacing(3),
	},
	submit: {
		margin: theme.spacing(3, 0, 2),
	},
}));

export default function SignUp(props) {
	const classes = useStyles();
	useEffect(() => {
		if (fakeAuth.isAuthenticated) {
			props.history.push('/categories');
		}
	})

	return (
		<Container component="main" maxWidth="xs">
			<CssBaseline />
			<div className={classes.paper}>
				<Avatar className={classes.avatar}>
					<LockOutlinedIcon />
				</Avatar>
				<Typography component="h1" variant="h5">
					Sign up
        </Typography>
				<Formik
					initialValues={{ username: '', email: '', password: '' }}
					onSubmit={async (values, actions) => {
						actions.setSubmitting(true);
						await register(values)
							.then(r => {
								toast.success("User registered successfully.");
								setTimeout(window.location.replace('/login'), 1000);
							})
							.catch(err => {
								toast.error(`${err.response.data} Status code: ${err.response.status}`);
								actions.setSubmitting(false);
							});

					}}
				>
					{(formProps) => {
						const { values, handleChange, setFieldTouched } = formProps;
						const change = (name, e) => {
							e.persist();
							handleChange(e);
							setFieldTouched(name, true, false);
						};
						return (
							<Form className={classes.form}>
								<Grid container spacing={2}>
									<Grid item xs={12}>
										<TextField
											autoComplete="uname"
											name="username"
											variant="outlined"
											required
											fullWidth
											id="username"
											label="Username"
											autoFocus
											value={values.username}
											onChange={change.bind(null, "username")}
										/>
									</Grid>
									<Grid item xs={12}>
										<TextField
											variant="outlined"
											required
											fullWidth
											id="email"
											label="Email Address"
											name="email"
											autoComplete="email"
											value={values.email}
											onChange={change.bind(null, "email")}
										/>
									</Grid>
									<Grid item xs={12}>
										<TextField
											variant="outlined"
											required
											fullWidth
											name="password"
											label="Password"
											type="password"
											id="password"
											autoComplete="current-password"
											values={values.password}
											onChange={change.bind(null, "password")}
										/>
									</Grid>
								</Grid>
								<Button
									type="submit"
									fullWidth
									variant="contained"
									color="primary"
									className={classes.submit}
									disabled={formProps.isSubmitting}
								>
									{"Sign Up"}
								</Button>
								<Grid container justify="flex-end">
									<Grid item>
										<RouteLink to="/login">
											<Link variant="body2">
												{"Already have an account? Sign in"}
											</Link>
										</RouteLink>
									</Grid>
								</Grid>
							</Form>
						);
					}}
				</Formik>
			</div>
			<Box mt={5}>
				<Copyright />
			</Box>
		</Container>
	);
}