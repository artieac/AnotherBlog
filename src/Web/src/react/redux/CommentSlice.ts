import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { IComment } from '../types/comment.types';
import CommentRepository from '../Repositories/CommentRepository';

interface CommentState {
    comments: IComment[];
    loading: boolean;
    error: string | null;
}

const initialState: CommentState = {
    comments: [],
    loading: false,
    error: null,
};

export const fetchCommentsByBlog = createAsyncThunk('comments/fetchByBlog', async ({ blogSubFolder, status }: { blogSubFolder: string, status?: string }) => {
    return await CommentRepository.getByBlog(blogSubFolder, status);
});

export const updateCommentStatus = createAsyncThunk('comments/updateStatus', async ({ blogSubFolder, postId, commentId, newState }: { blogSubFolder: string, postId: number, commentId: number, newState: string }) => {
    return await CommentRepository.updateState(blogSubFolder, postId, commentId, newState);
});

export const deleteComment = createAsyncThunk('comments/delete', async ({ blogSubFolder, postId, commentId }: { blogSubFolder: string, postId: number, commentId: number }) => {
    await CommentRepository.delete(blogSubFolder, postId, commentId);
    return commentId;
});

const commentSlice = createSlice({
    name: 'comments',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchCommentsByBlog.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(fetchCommentsByBlog.fulfilled, (state, action) => {
                state.loading = false;
                state.comments = action.payload;
            })
            .addCase(fetchCommentsByBlog.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || 'Failed to fetch comments';
            })
            .addCase(updateCommentStatus.fulfilled, (state, action) => {
                const index = state.comments.findIndex(c => c.Id === action.payload.Id);
                if (index !== -1) {
                    state.comments[index] = action.payload;
                }
            })
            .addCase(deleteComment.fulfilled, (state, action) => {
                state.comments = state.comments.filter(c => c.Id !== action.payload);
            });
    },
});

export default commentSlice.reducer;

