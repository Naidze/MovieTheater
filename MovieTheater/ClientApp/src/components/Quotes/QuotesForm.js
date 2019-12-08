import React from 'react';
import _ from 'lodash';
import {
	Dialog, DialogTitle, DialogContent, DialogActions, Button,
	TextField, Typography, ExpansionPanel, ExpansionPanelSummary,
	ExpansionPanelDetails,
	Container,
	List,
} from '@material-ui/core';
import { Formik, Form } from 'formik';
import { makeStyles } from '@material-ui/core/styles';
import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import Quote from './Quote';
import { createQuote } from '../../utils/networkFunctions';
import { toast } from 'react-toastify';

const useStyles = makeStyles(theme => ({
	heading: {
		fontSize: theme.typography.pxToRem(15),
		fontWeight: theme.typography.fontWeightRegular,
	},
	container: {
		marginTop: 20,
		maxHeight: '30vh',
		overflowY: 'auto'
	},
	root: {
		width: '100%',
		backgroundColor: theme.palette.background.paper,
	},
	inline: {
		display: 'inline',
	}
}));

export default function QuotesForm({ onCancel, movie }) {
	const classes = useStyles();

	const submitQuote = (values, actions) => {
		const quoteToSubmit = {
			title: values.quoteTilte,
			text: values.quote,
			movieID: movie.id
		};

		createQuote(quoteToSubmit)
			.then(r => {
				toast.success("Quote created successfully.");
				window.location.reload();
			})
			.catch(err => {
				toast.error(err.message);
				actions.setSubmitting(false);
			});
	}

	const quotesList = _.map(movie.quotes, (quote, id) => (
		<Quote key={id} quote={quote} />
	));

	return (
		<Dialog
			open
			onClose={onCancel}
			aria-labelledby="form-dialog-title"
			maxWidth="sm"
			fullWidth={true}
		>
			<DialogTitle id="form-dialog-title">
				{`${movie.title} Quotes`}
			</DialogTitle>
			<DialogContent>
				<ExpansionPanel>
					<ExpansionPanelSummary
						expandIcon={<ExpandMoreIcon />}
						aria-controls="panel1a-content"
						id="panel1a-header"
					>
						<Typography className={classes.heading}>
							{"Add new quote"}
						</Typography>
					</ExpansionPanelSummary>
					<ExpansionPanelDetails>
						<Formik
							initialValues={{ quoteTilte: '', quote: '' }}
							onSubmit={(values, actions) => {
								actions.setSubmitting(true);
								submitQuote(values, actions);
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
									<Form>
										<TextField
											variant="outlined"
											margin="normal"
											required
											fullWidth
											id="quoteTilte"
											label="Title"
											name="quoteTilte"
											value={values.quoteTilte}
											onChange={change.bind(null, "quoteTilte")}
										/>
										<TextField
											variant="outlined"
											margin="normal"
											required
											fullWidth
											id="quote"
											label="Quote"
											name="quote"
											value={values.quote}
											onChange={change.bind(null, "quote")}
										/>
										<Button color="primary" variant="contained" type="submit" disabled={formProps.isSubmitting}>
											{"Create"}
										</Button>
									</Form>
								);
							}}
						</Formik>
					</ExpansionPanelDetails>
				</ExpansionPanel>
				{_.isEmpty(movie.quotes)
					? <Container className={classes.container}>
						<Typography>
							{"No quotes created"}
						</Typography>
					</Container>
					: <Container className={classes.container}>
						<List className={classes.root}>
							{quotesList}
						</List>
					</Container>}
			</DialogContent>
			<DialogActions>
				<Button color="default" onClick={onCancel}>
					{"Cancel"}
				</Button>
			</DialogActions>
		</Dialog>
	);
}