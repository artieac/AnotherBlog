import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { IUser } from '../Models/IUser';
import UserRepository from '../Repositories/UserRepository';

interface UserState {
    users: IUser[];
    currentUser: IUser | null;
    loading: boolean;
    error: string | null;
}

const initialState: UserState = {
    users: [],
    currentUser: null,
    loading: false,
    error: null,
};

export const fetchUsers = createAsyncThunk('users/fetchAll', async () => {
    return await UserRepository.getAll();
});

export const fetchUserById = createAsyncThunk('users/fetchById', async (id: number) => {
    return await UserRepository.getById(id);
});

export const saveUser = createAsyncThunk('users/save', async (user: IUser) => {
    return await UserRepository.save(user);
});

export const deleteUser = createAsyncThunk('users/delete', async (id: number) => {
    await UserRepository.delete(id);
    return id;
});

const userSlice = createSlice({
    name: 'users',
    initialState,
    reducers: {
        setCurrentUser: (state, action: PayloadAction<IUser | null>) => {
            state.currentUser = action.payload;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(fetchUsers.pending, (state) => {
                state.loading = true;
            })
            .addCase(fetchUsers.fulfilled, (state, action) => {
                state.loading = false;
                state.users = action.payload;
            })
            .addCase(fetchUsers.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || 'Failed to fetch users';
            })
            .addCase(fetchUserById.fulfilled, (state, action) => {
                state.currentUser = action.payload;
            })
            .addCase(saveUser.fulfilled, (state, action) => {
                const index = state.users.findIndex(u => u.Id === action.payload.Id);
                if (index !== -1) {
                    state.users[index] = action.payload;
                } else {
                    state.users.push(action.payload);
                }
                state.currentUser = action.payload;
            })
            .addCase(deleteUser.fulfilled, (state, action) => {
                state.users = state.users.filter(u => u.Id !== action.payload);
            });
    },
});

export const { setCurrentUser } = userSlice.actions;
export default userSlice.reducer;
