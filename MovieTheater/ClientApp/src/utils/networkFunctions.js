import axios from 'axios';

const defaultOptions = {
	headers: {
		'Authorization': `Bearer ${localStorage.getItem('access_token')}`
	}
};

const instance = axios.create(defaultOptions);

export const getCategories = () => instance.get('/api/categories');
export const getCategory = categoryId => instance.get(`/api/categories/${categoryId}`);
export const createCategory = category => instance.post('/api/categories', category);
export const editCategory = (categoryId, category) => instance.put(`/api/categories/${categoryId}`, category);
export const deleteCategory = categoryId => instance.delete(`/api/categories/${categoryId}`);

export const getReview = movieId => instance.get(`/api/movies/${movieId}/review`)
export const createReview = review => instance.post('/api/reviews', review);
export const editReview = (reviewId, review) => instance.put(`/api/reviews${reviewId}`, review);
export const deleteReview = reviewId => instance.delete(`/api/reviews/${reviewId}`);