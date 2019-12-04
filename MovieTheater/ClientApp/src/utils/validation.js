import * as Yup from 'yup';

export const loginValidation = Yup.object().shape({
	username: Yup.string()
		.required('Username is required'),
	password: Yup.string()
		.required('Password is required')
});

export const categoryFormValidation = Yup.object().shape({
	title: Yup.string()
		.required("Category title is required")
})

export const reviewFormValidation = Yup.object().shape({
	comment: Yup.string()
		.required("Review comment is required"),
	stars: Yup.number()
		.required("Review stars are required")
})
