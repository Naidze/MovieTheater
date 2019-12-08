import React, { useState, useEffect } from 'react';
import { getMovies } from '../../utils/networkFunctions';
import { toast } from 'react-toastify';
import { Container, Grid, Box } from '@material-ui/core';
import CircularProgress from '@material-ui/core/CircularProgress';
import Fab from '@material-ui/core/Fab';
import AddIcon from '@material-ui/icons/Add';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Button from '@material-ui/core/Button';
import ReviewForm from "../Reviews/ReviewForm.js";
import _ from 'lodash';
import { makeStyles } from "@material-ui/core/styles";
import MovieForm from "../Movies/MovieForm.js";
import EditIcon from '@material-ui/icons/Edit';
import QuotesForm from "../Quotes/QuotesForm.js";
import Movie from './Movie';

const useStyles = makeStyles(theme => ({
	cardsFlex: {
		display: 'flex',
		justifyContent: 'space-between'
	}
}));

export default function Movies() {
	const [movies, setMovies] = useState(null);
	const [reviewFormVisible, setReviewFormVisible] = useState(false);
	const [movieFormVisible, setMovieFormVisible] = useState(false);
	const [quotesFormVisible, setQuotesFormVisible] = useState(false);
	const [formMovie, setFormMovie] = useState({});

	useEffect(() => {
		getMovies()
			.then(r => setMovies(r.data))
			.catch(err => toast.error(err.message));
	}, [setMovies])

	const showReviewForm = movie => {
		if (movie) {
			setFormMovie(movie);
		}
		setReviewFormVisible(true);
	}

	const hideReviewForm = () => {
		setFormMovie({});
		setReviewFormVisible(false);
	}

	const showMovieForm = movie => {
		if (movie) {
			setFormMovie(movie);
		}
		setMovieFormVisible(true);
	}

	const hideMovieForm = () => {
		setMovieFormVisible(false);
	}

	const showQuotesForm = movie => {
		if (movie) {
			setFormMovie(movie);
		}
		setQuotesFormVisible(true);
	}

	const hideQuotesForm = () => {
		setFormMovie({});
		setQuotesFormVisible(false);
	}

	if (movies) {
		var moviesList = movies.map((movie, id) => (
			<Movie key={id} movie={movie} showMovieForm={showMovieForm} showReviewForm={showReviewForm} showQuotesForm={showQuotesForm} />
		))
	}

	const classes = useStyles();
	return (
		<Container maxWidth='lg'>
			<Grid container>
				<Box display="flex" p={5} width="100%" justifyContent="center">
					{moviesList
						? <Container maxWidth='lg'>
							<Grid container spacing={3} className={classes.cardsFlex}>
								{moviesList}
							</Grid>
						</Container>
						: <Box p={5} justifyContent="center"><CircularProgress /></Box>}
				</Box>
			</Grid>
			{reviewFormVisible && <ReviewForm onCancel={hideReviewForm} movie={formMovie} />}
			{movieFormVisible && <MovieForm onCancel={hideMovieForm} movie={formMovie} />}
			{quotesFormVisible && <QuotesForm onCancel={hideQuotesForm} movie={formMovie} />}
		</Container>
	);
}