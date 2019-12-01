import React, { PureComponent } from "react"
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { withStyles } from '@material-ui/core/styles';
import { getCategories } from '../utils/networkFunctions.js';
import { Container, Box, Grid } from "@material-ui/core";
import CircularProgress from '@material-ui/core/CircularProgress';
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import IconButton from '@material-ui/core/IconButton';
import EditIcon from '@material-ui/icons/Edit';
import { Link } from 'react-router-dom';

const styles = theme => ({
	root: {
		minWidth: 300,
		maxWidth: 360,
		backgroundColor: theme.palette.background.paper,

	},
	spacing: 8
});

class Categories extends PureComponent {
	state = {};
	componentDidMount() {
		getCategories()
			.then(r => this.setState({ categories: r.data }))
			.catch(err => console.log(err))
	}

	handleCategoryOpen(category) {
	}

	render() {
		const { classes } = this.props;
		let categories = [];
		if (this.state.categories) {
			categories = this.state.categories.map((cat, id) => (
				<ListItem key={id} button component={Link} to={`/categories/${cat.id}`}>
					<ListItemText primary={cat.title} />
					<ListItemSecondaryAction>
						<IconButton edge="end" aria-label="delete">
							<EditIcon />
						</IconButton>
					</ListItemSecondaryAction>
				</ListItem>
			));
		}
		return (
			<Container maxWidth='md'>
				<Grid container justify="center">
					<Box p={1}>
						{this.state.categories
							? < List component="nav" className={classes.root} aria-label="contacts">
								{categories}
							</List>
							: <Box p={5}><CircularProgress /></Box>}
					</Box>
				</Grid>
			</Container>
		);
	}
};

export default withStyles(styles)(Categories);