import React, { PureComponent } from "react"
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import StarIcon from '@material-ui/icons/Star';
import { withStyles } from '@material-ui/core/styles';
import axios from 'axios';
import { getCategories } from '../utils/networkFunctions.js';
import { Container, Box, Grid } from "@material-ui/core";
import { spacing } from '@material-ui/system';
import CircularProgress from '@material-ui/core/CircularProgress';

const styles = theme => ({
	root: {
		width: '100%',
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

	render() {
		const { classes } = this.props;
		let categories = [];
		if (this.state.categories) {
			categories = this.state.categories.map((cat, id) => (
				<ListItem key={id} button>
					<ListItemText primary={cat.title} />
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