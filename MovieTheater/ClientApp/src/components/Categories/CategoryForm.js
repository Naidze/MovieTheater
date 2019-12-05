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
import { createCategory, editCategory } from '../../utils/networkFunctions';
import { toast } from 'react-toastify';

export default function CategoryForm({ category, onCancel, onSubmit, updateCategories }) {
	const [isEditing, setEditing] = useState(false);

	useEffect(() => {

		if (!_.isEmpty(category)) {
			setEditing(true);
		} else {
			setEditing(false);
		}
	}, [setEditing])

	if (!_.isEmpty(category)) {
		var initialValues = {
			title: category.title,
			description: category.description
		}
	}

	function updateCategoriesList(category) {
		updateCategories(category);
	}

	function submitCategory(values, isEditing) {
		const categoryToSubmit = {
			title: values.title,
			description: values.description
		};
		if (isEditing) {
			editCategory(category.id, categoryToSubmit)
				.then(r => toast.success("Category editted successfully"))
				.catch(err => toast.error(err.message));
		} else {
			createCategory(categoryToSubmit)
				.then(r => {
					toast.success("Category created successfully");
					updateCategoriesList(r.data);
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
				{!_.isEmpty(category) ? category.title : "Add Category"}
			</DialogTitle>
			<Formik
				initialValues={initialValues || {}}
				validationSchema={categoryFormValidation}
				onSubmit={(values, actions) => {
					actions.setSubmitting(true);
					submitCategory(values, isEditing);
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
							</DialogContent>
							<DialogActions>
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