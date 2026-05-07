import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { IBlogPost } from '../Models/IBlogPost';
import BlogPostRepository from '../Repositories/BlogPostRepository';

interface BlogPostState {
    posts: IBlogPost[];
    currentPost: IBlogPost | null;
    loading: boolean;
    error: string | null;
}

const initialState: BlogPostState = {
    posts: [],
    currentPost: null,
    loading: false,
    error: null,
};

export const fetchPostsByBlog = createAsyncThunk('posts/fetchByBlog', async (blogSubFolder: string) => {
    return await BlogPostRepository.getAllByBlog(blogSubFolder);
});

export const fetchPostById = createAsyncThunk('posts/fetchById', async ({ blogSubFolder, id }: { blogSubFolder: string, id: number }) => {
    return await BlogPostRepository.getById(blogSubFolder, id);
});

export const savePost = createAsyncThunk('posts/save', async ({ blogSubFolder, post }: { blogSubFolder: string, post: IBlogPost }) => {
    return await BlogPostRepository.save(blogSubFolder, post);
});

export const deletePost = createAsyncThunk('posts/delete', async ({ blogSubFolder, id }: { blogSubFolder: string, id: number }) => {
    await BlogPostRepository.delete(blogSubFolder, id);
    return id;
});

const blogPostSlice = createSlice({
    name: 'blogPosts',
    initialState,
    reducers: {
        setCurrentPost: (state, action: PayloadAction<IBlogPost | null>) => {
            state.currentPost = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchPostsByBlog.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchPostsByBlog.fulfilled, (state, action) => {
                state.loading = false;
                state.posts = action.payload;
            })
            .addCase(fetchPostsByBlog.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || 'Failed to fetch posts';
            })
            .addCase(fetchPostById.fulfilled, (state, action) => {
                state.currentPost = action.payload;
            })
            .addCase(savePost.fulfilled, (state, action) => {
                const index = state.posts.findIndex(p => p.Id === action.payload.Id);
                if (index !== -1) {
                    state.posts[index] = action.payload;
                } else {
                    state.posts.push(action.payload);
                }
                state.currentPost = action.payload;
            })
            .addCase(deletePost.fulfilled, (state, action) => {
                state.posts = state.posts.filter(p => p.Id !== action.payload);
            });
    },
});

export const { setCurrentPost } = blogPostSlice.actions;
export default blogPostSlice.reducer;
