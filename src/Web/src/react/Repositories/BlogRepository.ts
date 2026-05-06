import RestClient from './RestClient';
import { IBlog } from '../Models/IBlog';

class BlogRepository {
    public async getAll(): Promise<IBlog[]> {
        return RestClient.get<IBlog[]>('/api/Blogs');
    }

    public async getById(id: number): Promise<IBlog> {
        return RestClient.get<IBlog>(`/api/Blog/${id}`);
    }

    public async save(blog: IBlog): Promise<IBlog> {
        if (blog.Id && blog.Id > 0) {
            return RestClient.put<IBlog>(`/api/Blog/${blog.Id}`, blog);
        } else {
            return RestClient.post<IBlog>('/api/Blog', blog);
        }
    }
}

export default new BlogRepository();
