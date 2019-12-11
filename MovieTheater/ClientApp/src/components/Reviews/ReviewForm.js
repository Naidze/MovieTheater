import React, { useState, useEffect } from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import Button from '@material-ui/core/Button';
import Rating from '@material-ui/lab/Rating';
import { Formik, Form } from 'formik';
import { reviewFormValidation } from '../../utils/validation';
import { TextField, Typography, IconButton, InputAdornment } from '@material-ui/core';
import EditIcon from '@material-ui/icons/Edit';
import CloseIcon from '@material-ui/icons/Close';
import { makeStyles } from '@material-ui/styles';
import { createReview, editReview, deleteReview, getReview } from '../../utils/networkFunctions';
import { toast } from 'react-toastify';

const useStyles = makeStyles(theme => ({
	comment: {
		marginBottom: 10
	}
}));

export default function ReviewForm({ movie, onCancel, onSubmit }) {
	const classes = useStyles();
	const [review, setReview] = useState({});
	const [isEditingComment, setEditingComment] = useState(false);
	const [creatingNew, setCreatingNew] = useState(true);

	useEffect(() => {
		getReview(movie.id)
			.then(r => {
				setCreatingNew(false);
				setReview(r.data);
			})
			.catch(err => {
				if (err.response.status === 404) {
					setCreatingNew(true);
				} else {
					toast.error(err.message)
				}
			});
	}, [setReview, setCreatingNew]);

	function submitReview(values) {
		const reviewToSubmit = {
			comment: values.comment,
			stars: values.stars,
			movieId: movie.id
		};

		if (creatingNew) {
			createReview(reviewToSubmit)
				.then(r => {
					toast.success("Review created successfully");
					onCancel();
				})
				.catch(err => toast.err(err.message));
		} else {
			editReview(review.id, reviewToSubmit)
				.then(r => {
					toast.success("Review updated successfully");
					onCancel();
				})
				.catch(err => toast.error(err.message));
		}
	}

	function onDelete(id) {
		// eslint-disable-next-line no-restricted-globals
		const confirmed = confirm("Are you sure want to delete this review?");
		if (confirmed === true) {
			deleteReview(id)
				.then(r => {
					toast.success("Review deleted successfully");
					onCancel();
				})
				.catch(err => toast.error(err.message));
		}
	}

	return (
		<Dialog
			open
			onClose={onCancel}
			aria-labelledby="form-dialog-title"
			maxWidth="sm"
			fullWidth={true}
		>
			<DialogTitle id="form-dialog-title">
				{movie.title}
			</DialogTitle>
			<Formik
				enableReinitialize
				initialValues={{
					comment: review.comment,
					stars: !creatingNew ? review.stars : 0
				}}
				validationSchema={reviewFormValidation}
				onSubmit={(values, actions) => {
					actions.setSubmitting(true);
					submitReview(values);
					actions.setSubmitting(false);
				}}
			>
				{(formProps) => {
					const { values, handleChange, setFieldTouched, setFieldValue } = formProps;
					const change = (name, e) => {
						e.persist();
						handleChange(e);
						setFieldTouched(name, true, false);
					};
					if (!creatingNew) {
						var inputProps = {
							InputProps: {
								endAdornment: (
									<InputAdornment position="end">
										<IconButton onClick={() => setEditingComment(false)}>
											<CloseIcon />
										</IconButton>
									</InputAdornment>
								)
							}
						};
					}
					return (
						<Form>
							<DialogContent>
								{!isEditingComment && !creatingNew
									? (
										<Typography className={classes.comment}>
											{values.comment}
											<IconButton onClick={() => setEditingComment(true)}>
												<EditIcon />
											</IconButton>
										</Typography>
									)
									: (
										<TextField
											multiline
											variant="outlined"
											margin="normal"
											required
											fullWidth
											id="comment"
											label="Comment"
											name="comment"
											value={values.comment}
											onChange={change.bind(null, "comment")}
											{...inputProps}
										/>
									)}
								<Rating
									id="stars"
									label="Stars"
									name="stars"
									value={values.stars}
									onChange={(event, newValue) => setFieldValue("stars", newValue)}
								/>
							</DialogContent>
							<DialogActions>
								{!creatingNew && (
									<Button color="secondary" onClick={() => onDelete(review.id)}>
										{"Remove"}
									</Button>
								)}
								<div style={{ flex: '1 0 0' }} />
								<Button color="default" onClick={onCancel}>
									{"Cancel"}
								</Button>
								<Button color="primary" variant="contained" type="submit">
									{"Submit"}
								</Button>
							</DialogActions>
						</Form>
					);
				}}
			</Formik>
		</Dialog>
	);
}