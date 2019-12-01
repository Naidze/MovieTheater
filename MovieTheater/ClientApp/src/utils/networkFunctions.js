import axios from 'axios';

const defaultOptions = {
	headers: {
		'Authorization': `Bearer ${localStorage.getItem('access_token')}`
	}
};

const instance = axios.create(defaultOptions);

export const getCategories = () => instance.get('/api/categories');