import React, { useState, useEffect } from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import Button from '@material-ui/core/Button';
import _ from 'lodash';
import { Formik, Form } from 'formik';
import { categoryFormValidation } from '../../utils/validation';
import { TextField } from '@material-ui/core';
import { createCategory, editCategory, createMovie, editMovie, deleteMovie } from '../../utils/networkFunctions';
import { toast } from 'react-toastify';

export default function MovieForm({ movie, onCancel, onSubmit, updateCategories }) {
	const [isEditing, setEditing] = useState(false);

	useEffect(() => {
		if (!_.isEmpty(movie)) {
			setEditing(true);
		} else {
			setEditing(false);
		}
	}, [setEditing])

	if (!_.isEmpty(movie)) {
		const { author, title, description, year, rating, imageURL } = movie;
		var initialValues = {
			author,
			title,
			description,
			year,
			rating,
			imageURL
		}
	}

	function submitMovie(values, isEditing) {
		const { author, title, description, year, rating, imageURL, categoryId } = values
		const movieToSubmit = {
			author, title, description, year, rating, imageURL, categoryId
		};
		if (isEditing) {
			editMovie(movie.id, movieToSubmit)
				.then(document.location.reload())
				.catch(err => toast.error(err.message));
		} else {
			createMovie(movieToSubmit)
				.then(r => {
					toast.success("Movie created successfully");
					onCancel();
					setTimeout(window.location.reload(), 1000);
				})
				.catch(err => toast.error(err.message));
		}
	}

	const onDelete = movieId => {
		deleteMovie(movieId)
			.then(document.location.reload())
			.catch(err => toast.error(err.message));
	};

	return (
		<Dialog
			open
			onClose={onCancel}
			aria-labelledby="form-dialog-title"
			maxWidth="sm"
			fullWidth={true}
		>
			<DialogTitle id="form-dialog-title">
				{!_.isEmpty(movie) ? movie.title : "Add Movie"}
			</DialogTitle>
			<Formik
				initialValues={initialValues || {}}
				// validationSchema={categoryFormValidation}
				onSubmit={(values, actions) => {
					actions.setSubmitting(true);
					submitMovie(values, isEditing);
					actions.setSubmitting(false);
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
							<DialogContent>
								<TextField
									variant="outlined"
									margin="normal"
									required
									fullWidth
									id="author"
									label="Author"
									name="author"
									value={values.author}
									onChange={change.bind(null, "author")}
								/>
								<TextField
									variant="outlined"
									margin="normal"
									required
									fullWidth
									id="title"
									label="Title"
									name="title"
									value={values.title}
									onChange={change.bind(null, "title")}
								/>
								<TextField
									variant="outlined"
									multiline
									margin="normal"
									required
									fullWidth
									id="description"
									label="Description"
									name="description"
									value={values.description}
									onChange={change.bind(null, "description")}
								/>
								<TextField
									variant="outlined"
									type="number"
									margin="normal"
									fullWidth
									id="year"
									label="Year"
									name="year"
									value={values.year || ''}
									onChange={change.bind(null, "year")}
									inputProps={{ min: "1900", max: new Date().getFullYear(), step: "1" }}
								/>
								<TextField
									variant="outlined"
									type="number"
									margin="normal"
									fullWidth
									id="rating"
									label="Rating"
									name="rating"
									value={values.rating || ''}
									onChange={change.bind(null, "rating")}
									inputProps={{ min: "1900", max: new Date().getFullYear(), step: "1" }}
								/>
								<TextField
									variant="outlined"
									margin="normal"
									fullWidth
									id="imageURL"
									label="Image URL"
									name="imageURL"
									value={values.imageURL || ''}
									onChange={change.bind(null, "imageURL")}
								/>
							</DialogContent>
							<DialogActions>
								{isEditing && (
									<Button color="secondary" onClick={() => onDelete(movie.id)}>
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