import React, { Fragment, useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import Divider from '@material-ui/core/Divider';
import ListItemText from '@material-ui/core/ListItemText';
import ListItemAvatar from '@material-ui/core/ListItemAvatar';
import Avatar from '@material-ui/core/Avatar';
import Typography from '@material-ui/core/Typography';
import VideoCam from '@material-ui/icons/Videocam'
import { IconButton } from '@material-ui/core';
import RemoveIcon from '@material-ui/icons/Delete';
import EditIcon from '@material-ui/icons/Edit';
import { toast } from 'react-toastify';
import { deleteCinema } from '../../utils/networkFunctions';

const useStyles = makeStyles(theme => ({
	root: {
		width: '100%',
		backgroundColor: theme.palette.background.paper,
	},
	inline: {
		display: 'inline',
	},
	cinemaCard: {
		width: '100%'
	}
}));

export default function Cinema({ cinema, showCinemaForm }) {
	const [deleteDisabled, setDeleteDisabled] = useState(false);

	const onCinemaDelete = cinemaId => {
		setDeleteDisabled(true);
		// eslint-disable-next-line no-restricted-globals
		const confirmed = confirm(`Are you sure want to delete cinema ${cinema.name}?`);
		if (confirmed) {
			deleteCinema(cinemaId)
				.then(r => {
					toast.success('Cinema deleted successfully.')
					setTimeout(window.location.reload(), 1000);
				})
				.catch(err => {
					setDeleteDisabled(false);
					toast.error(err.message);
				});
		}
	}

	const classes = useStyles();
	return (
		<Fragment>
			<ListItem alignItems="flex-start" className={classes.cinemaCard}>
				<ListItemAvatar>
					<Avatar>
						<VideoCam />
					</Avatar>
				</ListItemAvatar>
				<ListItemText
					primary={cinema.name}
					secondary={
						<React.Fragment>
							<Typography
								component="span"
								variant="body2"
								className={classes.inline}
								color="textPrimary"
							>
								{cinema.capacity || ''}
							</Typography>
							{` — ${cinema.address}`}
						</React.Fragment>
					}
				/>
				<IconButton onClick={() => showCinemaForm(cinema)} >
					<EditIcon color="primary" />
				</IconButton>
				<IconButton onClick={() => onCinemaDelete(cinema.id)} disabled={deleteDisabled}>
					<RemoveIcon color="error" />
				</IconButton>
			</ListItem>
			<Divider component="li" />
		</Fragment>
	);
}