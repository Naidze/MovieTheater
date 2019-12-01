import React, { PureComponent } from "react"
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
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

const styles = theme => ({
	root: {
		minWidth: 300,
		maxWidth: 360,
		backgroundColor: theme.palette.background.paper,
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
		overflowY: 'scroll'
	}
});

class Category extends PureComponent {
	state = {};
	componentDidMount() {
		const { id } = this.props.match.params;
		getCategory(id)
			.then(r => this.setState({ category: r.data }))
			.catch(err => console.log(err))
	}

	handleCategoryOpen(category) {
	}

	render() {
		const { classes } = this.props;
		const { category } = this.state;
		return (
			<Container maxWidth='lg'>
				<Grid container>
					<Box display="flex" p={1} width="100%" justifyContent="center">
						{!this.state.category
							? <Box p={5} justifyContent="center"><CircularProgress /></Box>
							: (
								<Container maxWidth='lg'>
									<Typography variant="h2" component="h2" gutterBottom align="center">
										{category.title}
									</Typography>
									<Grid container spacing={3}>
										{category.movies.map((movie, id) => (
											<Grid item xs={12} key={id}>
												<Card className={classes.card}>
													<CardActionArea>
														<CardMedia
															className={classes.media}
															image={movie.imageURL || require('../content/images/movie-placeholder.jpg')}
															title="Contemplative Reptile"
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
														<Button size="small" color="primary">
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
			</Container>
		);
	}
};

export default withStyles(styles)(Category);