import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { IBlog } from '../types/blog.types';
import BlogRepository from '../Repositories/BlogRepository';

interface BlogState {
    blogs: IBlog[];
    currentBlog: IBlog | null;
    loading: boolean;
    error: string | null;
}

const initialState: BlogState = {
    blogs: [],
    currentBlog: null,
    loading: false,
    error: null,
};

export const fetchBlogs = createAsyncThunk('blogs/fetchAll', async () => {
    return await BlogRepository.getAll();
});

export const fetchBlogById = createAsyncThunk('blogs/fetchById', async (id: number) => {
    return await BlogRepository.getById(id);
});

export const saveBlog = createAsyncThunk('blogs/save', async (blog: IBlog) => {
    return await BlogRepository.save(blog);
});

const blogSlice = createSlice({
    name: 'blogs',
    initialState,
    reducers: {
        setCurrentBlog: (state, action: PayloadAction<IBlog | null>) => {
            state.currentBlog = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchBlogs.pending, (state) => {
                state.loading = true;
            })
            .addCase(fetchBlogs.fulfilled, (state, action) => {
                state.loading = false;
                state.blogs = action.payload;
            })
            .addCase(fetchBlogs.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || 'Failed to fetch blogs';
            })
            .addCase(fetchBlogById.fulfilled, (state, action) => {
                state.currentBlog = action.payload;
            })
            .addCase(saveBlog.fulfilled, (state, action) => {
                const index = state.blogs.findIndex(b => b.Id === action.payload.Id);
                if (index !== -1) {
                    state.blogs[index] = action.payload;
                } else {
                    state.blogs.push(action.payload);
                }
                state.currentBlog = action.payload;
            });
    },
});

export const { setCurrentBlog } = blogSlice.actions;
export default blogSlice.reducer;

