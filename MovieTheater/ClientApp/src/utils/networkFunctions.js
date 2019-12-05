import axios from 'axios';

const defaultOptions = {
	headers: {
		'Authorization': `Bearer ${localStorage.getItem('access_token')}`
	}
};

const instance = axios.create(defaultOptions);

// categories
export const getCategories = () => instance.get('/api/categories');
export const getCategory = categoryId => instance.get(`/api/categories/${categoryId}`);
export const createCategory = category => instance.post('/api/categories', category);
export const editCategory = (categoryId, category) => instance.put(`/api/categories/${categoryId}`, category);
export const deleteCategory = categoryId => instance.delete(`/api/categories/${categoryId}`);

// movies
export const getMovie = movieId => instance.get(`/api/movies/${movieId}`);
export const createMovie = movie => instance.post('/api/movies/', movie);
export const editMovie = (movieId, movie) => instance.put(`/api/movies/${movieId}`, movie);
export const deleteMovie = movieId => instance.delete(`/api/movies/${movieId}`);

// reviews
export const getReview = movieId => instance.get(`/api/movies/${movieId}/review`)
export const createReview = review => instance.post('/api/reviews', review);
export const editReview = (reviewId, review) => instance.put(`/api/reviews${reviewId}`, review);
export const deleteReview = reviewId => instance.delete(`/api/reviews/${reviewId}`);