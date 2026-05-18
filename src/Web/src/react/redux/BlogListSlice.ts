import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { IBlogList, IBlogListItem } from '../types/blog-list.types';
import BlogListRepository from '../Repositories/BlogListRepository';

interface BlogListState {
    lists: IBlogList[];
    currentList: IBlogList | null;
    loading: boolean;
    error: string | null;
}

const initialState: BlogListState = {
    lists: [],
    currentList: null,
    loading: false,
    error: null,
};

export const fetchListsByBlog = createAsyncThunk('blogLists/fetchByBlog', async (blogSubFolder: string) => {
    return await BlogListRepository.getByBlog(blogSubFolder);
});

export const fetchListById = createAsyncThunk('blogLists/fetchById', async ({ blogSubFolder, id }: { blogSubFolder: string, id: number }) => {
    return await BlogListRepository.getById(blogSubFolder, id);
});

export const saveList = createAsyncThunk('blogLists/save', async ({ blogSubFolder, list }: { blogSubFolder: string, list: IBlogList }) => {
    return await BlogListRepository.save(blogSubFolder, list);
});

export const deleteList = createAsyncThunk('blogLists/delete', async ({ blogSubFolder, id }: { blogSubFolder: string, id: number }) => {
    const success = await BlogListRepository.delete(blogSubFolder, id);
    if (success) {
        return id;
    }
    throw new Error('Failed to delete list');
});

export const saveListItem = createAsyncThunk('blogLists/saveItem', async ({ blogSubFolder, listId, item }: { blogSubFolder: string, listId: number, item: IBlogListItem }) => {
    return await BlogListRepository.saveItem(blogSubFolder, listId, item);
});

export const deleteListItem = createAsyncThunk('blogLists/deleteItem', async ({ blogSubFolder, listId, itemId }: { blogSubFolder: string, listId: number, itemId: number }) => {
    return await BlogListRepository.deleteItem(blogSubFolder, listId, itemId);
});

const blogListSlice = createSlice({
    name: 'blogLists',
    initialState,
    reducers: {
        setCurrentList: (state, action: PayloadAction<IBlogList | null>) => {
            state.currentList = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchListsByBlog.pending, (state) => {
                state.loading = true;
            })
            .addCase(fetchListsByBlog.fulfilled, (state, action) => {
                state.loading = false;
                state.lists = action.payload;
            })
            .addCase(fetchListsByBlog.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || 'Failed to fetch lists';
            })
            .addCase(fetchListById.fulfilled, (state, action) => {
                state.currentList = action.payload;
            })
            .addCase(saveList.fulfilled, (state, action) => {
                const index = state.lists.findIndex(l => l.Id === action.payload.Id);
                if (index !== -1) {
                    state.lists[index] = action.payload;
                } else {
                    state.lists.push(action.payload);
                }
                state.currentList = action.payload;
            })
            .addCase(deleteList.fulfilled, (state, action) => {
                state.lists = state.lists.filter(l => l.Id !== action.payload);
            })
            .addCase(saveListItem.fulfilled, (state, action) => {
                state.currentList = action.payload;
                const index = state.lists.findIndex(l => l.Id === action.payload.Id);
                if (index !== -1) {
                    state.lists[index] = action.payload;
                }
            })
            .addCase(deleteListItem.fulfilled, (state, action) => {
                state.currentList = action.payload;
                const index = state.lists.findIndex(l => l.Id === action.payload.Id);
                if (index !== -1) {
                    state.lists[index] = action.payload;
                }
            });
    },
});

export const { setCurrentList } = blogListSlice.actions;
export default blogListSlice.reducer;

