import { configureStore } from '@reduxjs/toolkit';
import blogReducer from './BlogSlice';
import siteReducer from './SiteSlice';
import userReducer from './UserSlice';
import blogPostReducer from './BlogPostSlice';
import commentReducer from './CommentSlice';
import blogListReducer from './BlogListSlice';

export const store = configureStore({
    reducer: {
        blogs: blogReducer,
        site: siteReducer,
        users: userReducer,
        blogPosts: blogPostReducer,
        comments: commentReducer,
        blogLists: blogListReducer,
    },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
