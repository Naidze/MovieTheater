import React, { PureComponent, useState, useEffect } from "react"
import List from '@material-ui/core/List';
import ListItemText from '@material-ui/core/ListItemText';
import { withStyles } from '@material-ui/core/styles';
import { getCategory } from '../utils/networkFunctions.js';
import { Container, Box, Grid, Typography, Paper } from "@material-ui/core";
import CircularProgress from '@material-ui/core/CircularProgress';
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import IconButton from '@material-ui/core/IconButton';
import EditIcon from '@material-ui/icons/Edit';
import { Link } from 'react-router-dom';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Button from '@material-ui/core/Button';
import ReviewForm from "./Reviews/ReviewForm.js";
import CategoryForm from "./Categories/CategoryForm.js";
import { ToastContainer, toast } from "react-toastify";
import _ from 'lodash';
import { makeStyles } from "@material-ui/styles";

const useStyles = makeStyles(theme => ({
	root: {
		minWidth: 300,
		maxWidth: 360,
	},
	card: {
		maxWidth: 345
	},
	media: {
		height: 200,
	},
	description: {
		textAlign: 'justify',
		height: 75,
		overflowY: 'auto'
	}
}));

export default function Category(props) {
	const classes = useStyles();
	const [reviewFormVisible, setReviewFormVisible] = useState(false);
	const [category, setCategory] = useState({});
	const [formMovie, setFormMovie] = useState({});

	const hideReviewForm = () => {
		setFormMovie({});
		setReviewFormVisible(false);
	}

	const showReviewForm = movie => {
		if (movie) {
			setFormMovie(movie);
		}
		setReviewFormVisible(true);
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
								<Typography variant="h3" component="h3" gutterBottom align="center">
									{category.title}
								</Typography>
								<Typography gutterBottom align="center">
									{category.description}
								</Typography>
								<Grid container spacing={3}>
									{category.movies.map((movie, id) => (
										<Grid item xs={12} key={id}>
											<Card className={classes.card}>
												<CardActionArea>
													<CardMedia
														className={classes.media}
														image={movie.imageURL || require('../content/images/movie-placeholder.jpg')}
														title={movie.title}
													/>
													<CardContent>
														<Typography gutterBottom variant="h4" component="h2">
															{movie.title}
														</Typography>
														<Typography gutterBottom variant="subtitle1" component="p">
															{movie.author}
														</Typography>
														<Typography
															variant="body2"
															color="textSecondary"
															component="p"
															className={classes.description}
														>
															{movie.description}
														</Typography>
													</CardContent>
												</CardActionArea>
												<CardActions>
													<Button size="small" color="primary" onClick={() => showReviewForm(movie)}>
														{"Review"}
													</Button>
													<Button size="small" color="primary">
														{"Quotes"}
													</Button>
												</CardActions>
											</Card>
										</Grid>
									))}
								</Grid>
							</Container>
						)}
				</Box>
			</Grid>
			{reviewFormVisible && <ReviewForm onCancel={hideReviewForm} movie={formMovie} />}
		</Container>
	);
};