import RestClient from './RestClient';
import { ISiteInfo } from '@/types/site-info.types';

class SiteRepository {
    public async get(): Promise<ISiteInfo> {
        try {
            return await RestClient.get<ISiteInfo>('/api/SiteInfo');
        } catch (error) {
            console.error('Failed to fetch site info:', error);
            throw new Error('Could not retrieve site information.');
        }
    }

    public async save(siteInfo: ISiteInfo): Promise<ISiteInfo> {
        try {
            return await RestClient.post<ISiteInfo>('/api/SiteInfo', siteInfo);
        } catch (error) {
            console.error('Failed to save site info:', error);
            throw new Error('Could not save site information.');
        }
    }
}

export default new SiteRepository();

