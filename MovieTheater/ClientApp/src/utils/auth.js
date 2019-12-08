import decode from 'jwt-decode';

export const parse = () => {
	const token = localStorage.getItem('ACCESS_TOKEN');
	try {
		const decoded = decode(token);
		if (decoded.exp > Date.now() / 1000) {
			return decoded;
		}

		localStorage.removeItem('ACCESS_TOKEN');
		return null;
	} catch (err) {
		return null;
	}
};

export const isAuth = () => {
	const token = localStorage.getItem('ACCESS_TOKEN');
	if (!token) {
		sessionStorage.removeItem("IS_AUTHENTICATED");
		return false;
	}
	const decoded = decode(token);
	if (decoded.exp > Date.now() / 1000) {
		sessionStorage.setItem("IS_AUTHENTICATED", true);
		return true;
	} else {

	}
};