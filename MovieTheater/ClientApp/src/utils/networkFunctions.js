import axios from 'axios';

const config = () => {
	return {
		headers: {
			'Authorization': `Bearer ${localStorage.getItem('ACCESS_TOKEN')}`
		}
	}
}

// auth
export const authenticate = () => axios.get('/api/auth', config());
export const login = data => axios.post('/api/auth/token', data);
export const register = data => axios.post('/api/users/register', data);

// categories
export const getCategories = () => axios.get('/api/categories', config());
export const getCategory = categoryId => axios.get(`/api/categories/${categoryId}`, config());
export const createCategory = category => axios.post('/api/categories', category, config());
export const editCategory = (categoryId, category) => axios.put(`/api/categories/${categoryId}`, category, config());
export const deleteCategory = categoryId => axios.delete(`/api/categories/${categoryId}`, config());

// movies
export const getMovies = () => axios.get('/api/movies', config())
export const getMovie = movieId => axios.get(`/api/movies/${movieId}`, config());
export const createMovie = movie => axios.post('/api/movies/', movie, config());
export const editMovie = (movieId, movie) => axios.put(`/api/movies/${movieId}`, movie, config());
export const deleteMovie = movieId => axios.delete(`/api/movies/${movieId}`, config());

// reviews
export const getReview = movieId => axios.get(`/api/movies/${movieId}/review`, config())
export const createReview = review => axios.post('/api/reviews', review, config());
export const editReview = (reviewId, review) => axios.put(`/api/reviews${reviewId}`, review, config());
export const deleteReview = reviewId => axios.delete(`/api/reviews/${reviewId}`, config());

//quotes

export const createQuote = quote => axios.post('/api/quotes', quote, config());
export const deleteQuote = quoteId => axios.delete(`/api/quotes/${quoteId}`, config());

// cinemas
export const getCinemas = () => axios.get('/api/cinemas', config());
export const getCinema = cinemaId => axios.get(`/api/cinemas/${cinemaId}`, config());
export const createCinema = cinema => axios.post('/api/cinemas', cinema, config());
export const editCinema = (cinemaId, cinema) => axios.put(`/api/cinemas/${cinemaId}`, cinema, config());
export const deleteCinema = cinemaId => axios.delete(`/api/cinemas/${cinemaId}`, config());