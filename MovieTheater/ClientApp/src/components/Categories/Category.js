import React, { useState, useEffect } from "react"
import { getCategory } from '../../utils/networkFunctions.js';
import { Container, Box, Grid, Typography } from "@material-ui/core";
import CircularProgress from '@material-ui/core/CircularProgress';
import Fab from '@material-ui/core/Fab';
import AddIcon from '@material-ui/icons/Add';
import ReviewForm from "../Reviews/ReviewForm.js";
import { toast } from "react-toastify";
import _ from 'lodash';
import { makeStyles } from "@material-ui/core/styles";
import MovieForm from "../Movies/MovieForm.js";
import QuotesForm from "../Quotes/QuotesForm.js";
import Movie from "../Movies/Movie.js";

const useStyles = makeStyles(theme => ({
	root: {
		minWidth: 300,
		maxWidth: 360,
	},
	card: {
		width: 300,
		height: 450,
		[theme.breakpoints.down('sm')]: {
			width: 200,
			height: 370
		},
	},
	media: {
		height: 200,
		[theme.breakpoints.down('sm')]: {
			height: 150,
			fontSize: 50
		},
	},
	description: {
		textAlign: 'justify',
		height: 75,
		overflowY: 'auto',
		[theme.breakpoints.down('sm')]: {
			height: 50,
		},
	},
	pageControls: {
		margin: 0,
		top: 'auto',
		right: 20,
		bottom: 20,
		left: 'auto',
		position: 'fixed'
	},
	editBtn: {
		margin: 0
	},
	cardsFlex: {
		display: 'flex',
		justifyContent: 'space-between'
	},
	categoryTitle: {
		[theme.breakpoints.down('xs')]: {
			fontSize: '2rem'
		},
	},
	categoryDescription: {
		[theme.breakpoints.down('xs')]: {
			fontSize: '.9rem'
		},
	},
	movieTitle: {
		whiteSpace: 'nowrap',
		[theme.breakpoints.down('sm')]: {
			fontSize: '1.6rem'
		},
	}
}));

export default function Category(props) {
	const classes = useStyles();
	const [reviewFormVisible, setReviewFormVisible] = useState(false);
	const [movieFormVisible, setMovieFormVisible] = useState(false);
	const [quotesFormVisible, setQuotesFormVisible] = useState(false);
	const [category, setCategory] = useState({});
	const [formMovie, setFormMovie] = useState({});

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

	useEffect(() => {
		const { id } = props.match.params;
		getCategory(id)
			.then(r => setCategory(r.data))
			.catch(err => toast.error(err.message))
	}, [setCategory])

	return (
		<Container maxWidth='lg'>
			<Grid container>
				<Box display="flex" p={1} width="100%" justifyContent="center">
					{_.isEmpty(category)
						? <Box p={5} justifyContent="center"><CircularProgress /></Box>
						: (
							<Container maxWidth='lg'>
								<Typography className={classes.categoryTitle} variant="h3" component="h3" gutterBottom align="center">
									{category.title}
								</Typography>
								<Typography className={classes.categoryDescription} gutterBottom align="center">
									{category.description}
								</Typography>
								<Grid container spacing={3} className={classes.cardsFlex}>
									{category.movies.map((movie, id) => (
										<Movie
											key={id}
											movie={movie}
											showReviewForm={showReviewForm}
											showQuotesForm={showQuotesForm}
											showMovieForm={showMovieForm}
										/>
									))}
								</Grid>
							</Container>
						)}
				</Box>
			</Grid>
			{reviewFormVisible && <ReviewForm onCancel={hideReviewForm} movie={formMovie} />}
			{movieFormVisible && <MovieForm onCancel={hideMovieForm} movie={formMovie} />}
			{quotesFormVisible && <QuotesForm onCancel={hideQuotesForm} movie={formMovie} />}
			<div className={classes.pageControls}>
				<Fab color="primary" aria-label="add" onClick={() => showMovieForm()}>
					<AddIcon />
				</Fab>
			</div>
		</Container>
	);
};