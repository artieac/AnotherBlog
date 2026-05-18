import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { ISiteInfo } from '../types/site-info.types';
import SiteRepository from '../Repositories/SiteRepository';

interface SiteState {
    siteInfo: ISiteInfo | null;
    loading: boolean;
    error: string | null;
}

const initialState: SiteState = {
    siteInfo: null,
    loading: false,
    error: null,
};

export const fetchSiteInfo = createAsyncThunk('site/fetch', async () => {
    return await SiteRepository.get();
});

export const saveSiteInfo = createAsyncThunk('site/save', async (siteInfo: ISiteInfo) => {
    return await SiteRepository.save(siteInfo);
});

const siteSlice = createSlice({
    name: 'site',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
            .addCase(fetchSiteInfo.pending, (state) => {
                state.loading = true;
            })
            .addCase(fetchSiteInfo.fulfilled, (state, action) => {
                state.loading = false;
                state.siteInfo = action.payload;
            })
            .addCase(fetchSiteInfo.rejected, (state, action) => {
                state.loading = false;
                state.error = action.error.message || 'Failed to fetch site info';
            })
            .addCase(saveSiteInfo.fulfilled, (state, action) => {
                state.siteInfo = action.payload;
            });
    },
});

export default siteSlice.reducer;

