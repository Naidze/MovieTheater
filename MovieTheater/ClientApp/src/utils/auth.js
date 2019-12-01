import decode from 'jwt-decode';

export const parse = () => {
	const token = localStorage.getItem('access_token');
	try {
		const decoded = decode(token);
		if (decoded.exp > Date.now() / 1000) {
			return decoded;
		}

		localStorage.removeItem('access_token');
		return null;
	} catch (err) {
		return null;
	}
};

export const isAuth = () => {
	const token = localStorage.getItem('access_token');
	if (!token) { return false; }

	return parse();
};