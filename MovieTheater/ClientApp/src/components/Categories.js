import React, { useState, useEffect } from "react"
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import { getCategories, deleteCategory } from '../utils/networkFunctions.js';
import { Container, Box, Grid, Button } from "@material-ui/core";
import CircularProgress from '@material-ui/core/CircularProgress';
import ListItemSecondaryAction from '@material-ui/core/ListItemSecondaryAction';
import IconButton from '@material-ui/core/IconButton';
import EditIcon from '@material-ui/icons/Edit';
import RemoveIcon from '@material-ui/icons/Delete';
import { Link } from 'react-router-dom';
import CategoryForm from "./Categories/CategoryForm.js";
import { makeStyles } from "@material-ui/styles";
import _ from 'lodash';
import { toast } from "react-toastify";

const useStyles = makeStyles(theme => ({
	root: {
		minWidth: 300,
		maxWidth: 360,

	},
	spacing: 8
}));

export default function Categories(props) {
	const classes = useStyles();
	const [categoryFormVisible, setCategoryFormVisible] = useState(false);
	const [categories, setCategories] = useState([]);
	const [formCategory, setFormCategory] = useState({});

	useEffect(() => {
		getCategories()
			.then(r => setCategories(r.data))
			.catch(err => console.log(err))
	}, [setCategories])

	const hideCategoryForm = () => {
		setFormCategory({});
		setCategoryFormVisible(false);
	}

	const showCategoryFrom = category => {
		if (category) {
			setFormCategory(category);
		}
		setCategoryFormVisible(true);
	}

	function onCategoryDelete(category) {
		deleteCategory(category.id)
			.then(r => {
				toast.success(`Category '${category.title}' deleted successfully.`)
				setCategories(_.filter(categories, cat => cat.id !== category.id))
			})
			.catch(err => toast.error(err.message));
	}

	function onCategorySubmit(category) {
		const tempCategories = _.slice(categories);
		tempCategories.unshift(category);
		setCategories(tempCategories);
	}

	var categoriesList = categories.map((cat, id) => (
		<ListItem key={id} button component={Link} to={`/categories/${cat.id}`}>
			<ListItemText primary={cat.title} />
			<ListItemSecondaryAction>
				<IconButton edge="end" aria-label="edit" onClick={() => showCategoryFrom(cat)}>
					<EditIcon />
				</IconButton>
				<IconButton edge="end" aria-label="delete" onClick={() => onCategoryDelete(cat)}>
					<RemoveIcon />
				</IconButton>
			</ListItemSecondaryAction>
		</ListItem >
	));
	return (
		<Container maxWidth='lg'>
			<Box m={2}>
				<Button color="primary" variant="contained" onClick={() => showCategoryFrom(null)}>Create</Button>
			</Box>
			<Grid container justify="center">
				<Box p={1}>
					{!_.isEmpty(categories)
						? < List component="nav" className={classes.root} aria-label="contacts">
							{categoriesList}
						</List>
						: <Box p={5}><CircularProgress /></Box>}
				</Box>
			</Grid>
			{categoryFormVisible && <CategoryForm onCancel={hideCategoryForm} category={formCategory} updateCategories={onCategorySubmit} />}
		</Container>
	);
}