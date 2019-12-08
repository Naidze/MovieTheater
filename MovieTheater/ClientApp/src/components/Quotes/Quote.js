import React, { useState, Fragment } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import {
	ListItem,
	ListItemText,
	IconButton,
	Divider
} from '@material-ui/core';
import RemoveIcon from '@material-ui/icons/Delete';
import { deleteQuote } from '../../utils/networkFunctions';
import { toast } from 'react-toastify';

const useStyles = makeStyles(theme => ({

}));

export default function Quote({ quote }) {
	const [deleteDisabled, setDeleteDisabled] = useState(false);

	const onQuoteDelete = quoteId => {
		setDeleteDisabled(true);
		deleteQuote(quoteId)
			.then(r => {
				toast.success('Quote deleted successfully');
				setTimeout(window.location.reload(), 1000);
			})
			.catch(err => {
				setDeleteDisabled(false);
				toast.error(err.message);
			});
	}

	return (
		<Fragment>
			<ListItem alignItems="flex-start">
				<ListItemText
					primary={quote.title}
					secondary={quote.text}
				/>
				<IconButton onClick={() => onQuoteDelete(quote.id)} disabled={deleteDisabled}>
					<RemoveIcon color="error" />
				</IconButton>
			</ListItem>
			<Divider component="li" />
		</Fragment>
	);
};