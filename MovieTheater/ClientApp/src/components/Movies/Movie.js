import React from 'react';
import Card from '@material-ui/core/Card';
import CardActionArea from '@material-ui/core/CardActionArea';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import CardMedia from '@material-ui/core/CardMedia';
import Button from '@material-ui/core/Button';
import { makeStyles } from "@material-ui/core/styles";
import EditIcon from '@material-ui/icons/Edit';
import { Grid, Typography, IconButton } from '@material-ui/core';

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
			height: 330
		},
	},
	media: {
		height: 200,
		[theme.breakpoints.down('sm')]: {
			height: 120,
			fontSize: 50
		},
	},
	movieTitle: {
		whiteSpace: 'nowrap',
		[theme.breakpoints.down('sm')]: {
			fontSize: '1.6rem'
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
	}
}));

export default function Movie({ movie, showReviewForm, showMovieForm, showQuotesForm }) {
	const classes = useStyles();
	return (
		<Grid item>
			<Card className={classes.card}>
				<CardActionArea>
					<CardMedia
						className={classes.media}
						image={movie.imageURL || require('../../content/images/movie-placeholder.jpg')}
						title={movie.title}
					/>
					<CardContent>
						<Typography className={classes.movieTitle} gutterBottom variant="h4" component="h2">
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
					<Button size="small" color="primary" onClick={() => showQuotesForm(movie)}>
						{"Quotes"}
					</Button>
					<div style={{ flex: '1 0 0' }} />
					<IconButton className={classes.editBtn} edge="end" aria-label="edit" onClick={() => showMovieForm(movie)} >
						<EditIcon />
					</IconButton>
				</CardActions>
			</Card>
		</Grid>
	);
}