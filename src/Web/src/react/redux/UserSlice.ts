import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import { IUser } from '../Models/IUser';
import UserRepository from '../Repositories/UserRepository';

interface UserState {
    users: IUser[];
    loggedInUser: IUser | null;
    selectedUser: IUser | null;
    loading: boolean;
    error: string | null;
}

const initialState: UserState = {
    users: [],
    loggedInUser: null,
    selectedUser: null,
    loading: false,
    error: null,
};

export const fetchUsers = createAsyncThunk('users/fetchAll', async () => {
    return await UserRepository.getAll();
});

export const fetchUserById = createAsyncThunk('users/fetchById', async (id: number) => {
    return await UserRepository.getById(id);
});

export const fetchCurrentUser = createAsyncThunk('users/fetchCurrent', async () => {
    return await UserRepository.getCurrent();
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
        setLoggedInUser: (state, action: PayloadAction<IUser | null>) => {
            state.loggedInUser = action.payload;
        },
        setSelectedUser: (state, action: PayloadAction<IUser | null>) => {
            state.selectedUser = action.payload;
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
                state.selectedUser = action.payload;
            })
            .addCase(fetchCurrentUser.fulfilled, (state, action) => {
                state.loggedInUser = action.payload;
            })
            .addCase(saveUser.fulfilled, (state, action) => {
                const index = state.users.findIndex(u => u.Id === action.payload.Id);
                if (index !== -1) {
                    state.users[index] = action.payload;
                } else {
                    state.users.push(action.payload);
                }
                state.selectedUser = action.payload;
                // If the user just edited themselves, update loggedInUser too
                if (state.loggedInUser && state.loggedInUser.Id === action.payload.Id) {
                    state.loggedInUser = action.payload;
                }
            })
            .addCase(deleteUser.fulfilled, (state, action) => {
                state.users = state.users.filter(u => u.Id !== action.payload);
            });
    },
});

export const { setLoggedInUser, setSelectedUser } = userSlice.actions;
export default userSlice.reducer;
