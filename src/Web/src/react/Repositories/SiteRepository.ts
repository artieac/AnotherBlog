import RestClient from './RestClient';
import { ISiteInfo } from '../Models/ISiteInfo';

class SiteRepository {
    public async get(): Promise<ISiteInfo> {
        return RestClient.get<ISiteInfo>('/api/SiteInfo');
    }

    public async save(siteInfo: ISiteInfo): Promise<ISiteInfo> {
        return RestClient.post<ISiteInfo>('/api/SiteInfo', siteInfo);
    }
}

export default new SiteRepository();
