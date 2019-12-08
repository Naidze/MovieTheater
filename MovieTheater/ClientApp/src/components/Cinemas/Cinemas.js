import React, { useState, useEffect } from 'react';
import _ from 'lodash';
import { Typography, Container, Box, Button, CircularProgress, Grid, List } from '@material-ui/core';
import CinemaForm from './CinemaForm';
import { getCinemas } from '../../utils/networkFunctions';
import { toast } from 'react-toastify';
import Cinema from './Cinema';

export default function Cinemas() {
	const [cinemas, setCinemas] = useState();
	const [cinemaFormVisible, setCinemaFormVisible] = useState(false);
	const [formCinema, setFormCinema] = useState();

	useEffect(() => {
		getCinemas()
			.then(r => setCinemas(r.data))
			.catch(err => toast.error(err.message));
	}, [setCinemas])

	const showCinemaForm = cinema => {
		if (cinema) {
			setFormCinema(cinema);
		}
		setCinemaFormVisible(true);
	}

	const hideCinemaForm = () => {
		setFormCinema();
		setCinemaFormVisible(false);
	}

	if (cinemas) {
		var cinemasList = (
			<Box p={1} width="100%">
				<List>
					{_.map(cinemas, (cinema, id) => (
						<Cinema key={id} cinema={cinema} showCinemaForm={showCinemaForm} />
					))}
				</List>
			</Box>
		);
	}

	return (
		<Container maxWidth="lg">
			<Box m={2}>
				<Button color="primary" variant="contained" onClick={() => showCinemaForm(null)}>Create</Button>
			</Box>
			<Grid container justify="center">
				{cinemas
					? !_.isEmpty(cinemas)
						? cinemasList
						: <Typography>
							{"No cinemas created"}
						</Typography>
					: <Box p={5} justifyContent="center"><CircularProgress /></Box>}
				{cinemaFormVisible && <CinemaForm onCancel={hideCinemaForm} cinema={formCinema} />}
			</Grid>
		</Container>
	);
}