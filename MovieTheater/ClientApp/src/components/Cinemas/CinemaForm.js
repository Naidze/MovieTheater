import React, { useState, useEffect } from 'react';
import _ from 'lodash';
import { Dialog, DialogContent, DialogActions, Button, TextField, DialogTitle } from '@material-ui/core';
import { deleteCinema, editCinema, createCinema } from '../../utils/networkFunctions';
import { toast } from 'react-toastify';
import { Formik, Form } from 'formik';

export default function CinemaForm({ onCancel, cinema }) {
	const [isEditing, setEditing] = useState(false);

	useEffect(() => {
		if (!_.isEmpty(cinema)) {
			setEditing(true);
		} else {
			setEditing(false);
		}
	}, [setEditing])

	if (!_.isEmpty(cinema)) {
		const { name, address, capacity } = cinema;
		var initialValues = {
			name, address, capacity
		}
	}

	const submitCinema = (values, isEditing) => {
		const { name, address, capacity } = values;
		const cinemaToSubmit = {
			name, address, capacity
		}

		if (isEditing) {
			editCinema(cinema.id, cinemaToSubmit)
				.then(r => {
					toast.success("Cinema updated successfully.");
					setTimeout(window.location.reload(), 1000);
				})
				.catch(err => toast.error(err.message));
		} else
			createCinema(cinemaToSubmit)
				.then(r => {
					toast.success("Cinema created successfully.");
					onCancel();
					setTimeout(window.location.reload(), 1000);
				})
				.catch(err => toast.error(err.message));
	}

	const onDelete = cinemaId => {
		// eslint-disable-next-line no-restricted-globals
		const confirmed = confirm(`Are you sure want to delete cinema ${cinema.name}?`);
		if (confirmed) {
			deleteCinema(cinemaId)
				.then(r => {
					toast.success('Cinema deleted successfully.');
					onCancel();
					setTimeout(window.location.reload(), 1000);
				})
				.catch(err => toast.error(err.message));
		}
	}

	console.log(cinema);
	return (
		<Dialog
			open
			onClose={onCancel}
			aria-labelledby="form-dialog-title"
			maxWidth="sm"
			fullWidth={true}
		>
			<DialogTitle id="form-dialog-title">
				{!_.isEmpty(cinema) ? cinema.name : "Add Cinema"}
			</DialogTitle>
			<Formik
				initialValues={initialValues || {}}
				onSubmit={(values, actions) => {
					actions.setSubmitting(true);
					submitCinema(values, isEditing);
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
									id="name"
									label="Name"
									name="name"
									value={values.name}
									onChange={change.bind(null, "name")}
								/>
								<TextField
									variant="outlined"
									margin="normal"
									required
									fullWidth
									id="address"
									label="Address"
									name="address"
									value={values.address}
									onChange={change.bind(null, "address")}
								/>
								<TextField
									type="number"
									variant="outlined"
									margin="normal"
									required
									fullWidth
									id="capacity"
									label="Capacity"
									name="capacity"
									value={values.capacity}
									onChange={change.bind(null, "capacity")}
									inputProps={{ min: "0", step: "1" }}
								/>
							</DialogContent>
							<DialogActions>
								{isEditing && (
									<Button color="secondary" onClick={() => onDelete(cinema.id)}>
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